using SemVer;
using SharpService.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace SharpService.ServiceDiscovery
{
    public class InMemoryDiscoveryProvider : ServiceDiscoveryProvider, IServiceDiscoveryProvider
    {
        private const string serviceConfig = "serviceGroup/serviceConfig";
        private readonly List<ServiceConfiguration> serviceConfigurations = ConfigurationManager.GetSection(serviceConfig) as List<ServiceConfiguration>;

        private readonly List<RegistryInformation> _serviceInstances = new List<RegistryInformation>();

        public List<RegistryInformation> ServiceInstances
        {
            get { return _serviceInstances; }
            set
            {
                foreach (var registryInformation in value)
                {
                    string url = registryInformation.Address;
                    if (registryInformation.Port >= 0)
                    {
                        url += $":{registryInformation.Port}";
                    }
                    RegisterServiceAsync(registryInformation.Name, registryInformation.Version, new Uri(url), tags: registryInformation.Tags);
                }
            }
        }

        public async Task RegisterServiceAsync()
        {
            foreach (var serviceConfig in serviceConfigurations)
                await RegisterServiceAsync(serviceConfig);
        }

        public async Task<RegistryInformation> RegisterServiceAsync(ServiceConfiguration serviceConfig)
        {
            var serviceName = await GetServiceName(serviceConfig.Interface, serviceConfig.Assembly);
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

        public Task<RegistryInformation> RegisterServiceAsync(string serviceName, string version, Uri uri, List<string> tags = null)
        {
            var tagList = (tags ?? Enumerable.Empty<string>()).ToList();

            var registryInformation = new RegistryInformation
            {
                Name = serviceName,
                Id = Guid.NewGuid().ToString(),
                Address = uri.ToString(),
                Host = uri.Host,
                Port = uri.Port,
                Version = version,
                Tags = tags ?? default(List<string>)
            };
            ServiceInstances.Add(registryInformation);
            return Task.FromResult(registryInformation);
        }

        public async Task<bool> DeregisterServiceAsync(string serviceId)
        {
            var instance = (await FindServicesAsync()).FirstOrDefault(x => x.Id == serviceId);
            if (instance != null)
            {
                ServiceInstances.Remove(instance);
                return true;
            }
            return false;
        }

        public Task<bool> DeregisterServiceAsync()
        {
            ServiceInstances = new List<RegistryInformation>();
            return Task.FromResult(true);
        }

        public Task<List<RegistryInformation>> FindServicesAsync()
        {
            return Task.FromResult(ServiceInstances);
        }

        public async Task<List<RegistryInformation>> FindServicesAsync(string name)
        {
            var instances = await FindServicesAsync();
            return instances.Where(x => x.Name == name).ToList();
        }

        public async Task<List<RegistryInformation>> FindServicesWithVersionAsync(string name, string version)
        {
            var instances = await FindServicesAsync(name);
            var range = new Range(version);
            return instances.Where(x => range.IsSatisfied(x.Version)).ToList();
        }

        public Task<List<RegistryInformation>> FindAllServicesAsync()
        {
            return Task.FromResult(ServiceInstances);
        }

    }
}
