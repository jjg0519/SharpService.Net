using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpService.Configuration;
using ZKClientNET.Client;
using ZKClientNET.Serialize;
using System.Configuration;
using ZooKeeperNet;
using SemVer;
using Org.Apache.Zookeeper.Data;
using ZKClientNET.Listener;
using System.Collections.Concurrent;

namespace SharpService.ServiceDiscovery
{
    public class ZooKeeperDiscoveryProvider : ServiceDiscoveryProvider, IServiceDiscoveryProvider
    {
       private ZKClient zkClient { set; get; }

        private const string registryConfig = "serviceGroup/registryConfig";
        private readonly RegistryConfiguration registryConfiguration = ConfigurationManager.GetSection(registryConfig) as RegistryConfiguration;

        private ConcurrentDictionary<string, IZKChildListener> serviceListeners = new ConcurrentDictionary<string, IZKChildListener>();
        private ConcurrentDictionary<string, IZKDataListener> commandListeners = new ConcurrentDictionary<string, IZKDataListener>();

        public ZooKeeperDiscoveryProvider()
        {
            zkClient = ZKClientBuilder.NewZKClient(registryConfiguration.Address)
                           .ConnectionTimeout(registryConfiguration.ConnectTimeout)
                           .Serializer(new SerializableSerializer())
                           .Build();

            IZKStateListener listener = new ZKStateListener()
                                                       .NewSession(() =>
                                                       {
                                                           ReconnectClient();
                                                       });
            zkClient.SubscribeStateChanges(listener);
        }

        private void ReconnectClient()
        {
            
        }

        public async Task RegisterServiceAsync()
        {
            foreach (var serviceConfig in serviceConfigurations.Where(x => x.Enable))
                await RegisterServiceAsync(serviceConfig);
        }

        public async Task<RegistryInformation> RegisterServiceAsync(ServiceConfiguration serviceConfig)
        {
            var serviceName = await GetServiceNameAsync(serviceConfig.Interface, serviceConfig.Assembly);
            var version = serviceConfig.Version;
            var uri = new Uri(serviceConfig.Address);
            var tags = new List<string>()
            {
                serviceConfig.Binding,
                serviceConfig.Security.ToString(),
                serviceConfig.Export
            };
            return await RegisterServiceAsync(serviceName, version, uri, tags);
        }

        public  async Task<RegistryInformation> RegisterServiceAsync(string serviceName, string version, Uri uri, List<string> tags = null)
        {
            var tagList = (tags ?? Enumerable.Empty<string>()).ToList();

            var registryInformation = new RegistryInformation
            {
                Name = serviceName,
                Id = await GetServiceIdAsync(serviceName, uri),
                Address = uri.ToString(),
                Host = uri.Host,
                Port = uri.Port,
                Version = version,
                Tags = tags ?? default(List<string>)
            };

            var zooServiceNamePath = $"/{SERVERPATH_PREFIX}/{serviceName}";
            var zooServiceIdPath = $"/{SERVERPATH_PREFIX}/{serviceName}/{registryInformation.Id}";
            if (!zkClient.Exists($"/{SERVERPATH_PREFIX}"))
            {
                zkClient.Create($"/{SERVERPATH_PREFIX}", true, CreateMode.Persistent);
            }
            if (!zkClient.Exists(zooServiceNamePath))
            {
                zkClient.Create(zooServiceNamePath, true, CreateMode.Persistent);
            }
            if (!zkClient.Exists(zooServiceIdPath))
            {
                zkClient.Create(zooServiceIdPath, registryInformation, CreateMode.Persistent);
            }
            else
            {
                zkClient.WriteData(zooServiceIdPath, registryInformation);
            }
            return registryInformation;
        }

        public async Task<bool> DeregisterServiceAsync(string serviceId)
        {
            var serviceName = await GetServiceNameAsync(serviceId);
            var zooServiceIdPath = $"/{SERVERPATH_PREFIX}/{serviceName}/{serviceId}";
            return zkClient.Delete(zooServiceIdPath);
        }

        public async Task<bool> DeregisterServiceAsync()
        {
            foreach (var serviceConfig in serviceConfigurations)
            {
                var serviceName = await GetServiceNameAsync(serviceConfig.Interface, serviceConfig.Assembly);
                var zooServiceNamPath = $"/{SERVERPATH_PREFIX}/{serviceName}";
                zkClient.DeleteRecursive(zooServiceNamPath);
            }
            return true;
        }

        public Task<List<RegistryInformation>> FindServicesAsync()
        {
            if (ServiceInstances.Count() == 0)
            {
                var zooServiceNames = zkClient.GetChildren($"/{SERVERPATH_PREFIX}");
                foreach (var zooServiceName in zooServiceNames)
                {
                    var zooServiceNamePath = $"/{SERVERPATH_PREFIX}/{zooServiceName}";

                    var zooServiceIds = zkClient.GetChildren(zooServiceNamePath);
                    foreach (var zooServiceId in zooServiceIds)
                    {
                        var zooServiceIdPath = $"/{SERVERPATH_PREFIX}/{zooServiceName}/{zooServiceId}";
                        Stat stat = new Stat();
                        //获取 节点中的对象  
                        var instance = zkClient.ReadData<RegistryInformation>(zooServiceIdPath, stat);
                        if (!ServiceInstances.Exists(x => x.Id == instance.Id))
                        {
                            ServiceInstances.Add(instance);
                        }                       
                    }
                }
            }
            return Task.FromResult(ServiceInstances);
        }

        public async Task<List<RegistryInformation>> FindServicesAsync(string serviceName)
        {
            var instances = await FindServicesAsync();
            if (!instances.Exists(x => x.Name == serviceName))
            {
                var zooServiceNamePath = $"/{SERVERPATH_PREFIX}/{serviceName}";               
                var zooServiceIds = zkClient.GetChildren(zooServiceNamePath);
                foreach (var zooServiceId in zooServiceIds)
                {
                    var zooServiceIdPath = $"/{SERVERPATH_PREFIX}/{serviceName}/{zooServiceId}";
                    Stat stat = new Stat();
                    //获取 节点中的对象  
                    var instance = zkClient.ReadData<RegistryInformation>(zooServiceIdPath, stat);
                    if (!ServiceInstances.Exists(x => x.Id == instance.Id))
                    {
                        ServiceInstances.Add(instance);
                    }
                }
            }
            return instances.Where(x => x.Name == serviceName).ToList();
        }

        private void AddServiceListener(string zooServiceNamePath)
        {
            IZKChildListener serviceListener = new ZKChildListener()
                .ChildChange((parentPath, currentChilds) =>
                {
                    Console.Write($"ChildChange:{parentPath}-{string.Join(",", currentChilds)}");
                })
                .ChildCountChanged((parentPath, currentChilds) =>
                {
                    Console.Write($"ChildCountChanged:{parentPath}-{string.Join(",", currentChilds)}");
                });
            if (!serviceListeners.ContainsKey(zooServiceNamePath))
            {
                serviceListeners.TryAdd(zooServiceNamePath, serviceListener);
                zkClient.SubscribeChildChanges(zooServiceNamePath, serviceListener);
            }
        }

        private void AddCommandListener(string zooServiceIdPath)
        {
            IZKDataListener commandListener = new ZKDataListener()
                 .DataCreated((path, data) =>
                 {
                     Console.Write($"DataCreated:{path}");
                 })
                .DataChange((path, data) =>
                {
                    Console.Write($"DataChange:{path}");
                })
                 .DataDeleted((path) =>
                 {
                     Console.Write($"DataDeleted:{path}");
                 });
            if (!serviceListeners.ContainsKey(zooServiceIdPath))
            {
                commandListeners.TryAdd(zooServiceIdPath, commandListener);
                zkClient.SubscribeDataChanges(zooServiceIdPath, commandListener);
            }
        }

        public async Task<List<RegistryInformation>> FindServicesWithVersionAsync(string name, string version)
        {
            var instances = await FindServicesAsync(name);
            var range = new Range(version);
            return instances.Where(x => range.IsSatisfied(x.Version)).ToList();
        }

        public Task<string> GetServiceNameAsync(string serviceId)
        {
            var names = serviceId.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)[0];
            return Task.FromResult($"{names[0]}{names[1]}{names[2]}") ;
        }
    }
}
