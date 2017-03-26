using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpService.Configuration;
using SemVer;
using SharpService.Components;
using SharpService.Utilities;

namespace SharpService.ServiceDiscovery
{
    public class InMemoryDiscoveryProvider : IServiceDiscoveryProvider
    {
        private IConfigurationObject configuration { get; set; }

        public InMemoryDiscoveryProvider()
        {
            configuration = ObjectContainer.Resolve<IConfigurationObject>();
        }

        private readonly List<RegistryInformation> ServiceInstances = new List<RegistryInformation>();

        public Task<List<RegistryInformation>> FindAllServicesAsync()
        {
            return Task.FromResult(ServiceInstances);
        }

        public Task<List<RegistryInformation>> FindServicesAsync(string name)
        {
            return Task.FromResult(ServiceInstances.Where(x => x.Name == name).ToList());
        }

        public async Task<List<RegistryInformation>> FindServicesWithVersionAsync(string name, string version)
        {
            var instances = await FindServicesAsync(name);
            var range = new Range(version);
            return instances.Where(x => range.IsSatisfied(x.Version)).ToList();
        }

        public async Task RegisterServiceAsync()
        {
            foreach (var serviceConfiguration in configuration.serviceConfigurations)
            {
                var protocolConfiguration = string.IsNullOrEmpty(serviceConfiguration.Protocol) ?
                                                           configuration.protocolConfigurations.FirstOrDefault(x => x.Defalut) :
                                                           configuration.protocolConfigurations.FirstOrDefault(x => x.Name == serviceConfiguration.Protocol);
                await RegisterServiceAsync(serviceConfiguration, protocolConfiguration, null, null);
            }
        }

        public async Task<RegistryInformation> RegisterServiceAsync(ServiceConfiguration serviceConfiguration, ProtocolConfiguration protocolConfiguration, Uri healthCheckUri = null, IEnumerable<string> tags = null)
        {
            var serviceId = ServiceDiscoveryHelper.GetServiceId(serviceConfiguration.Interface, serviceConfiguration.Port.ToString());
            var serviceName = ServiceDiscoveryHelper.GetServiceName(protocolConfiguration.Protocol, serviceConfiguration.Interface);

            string versionLabel = $"{ServiceDiscoveryHelper.VERSION_PREFIX}{serviceConfiguration.Version}";
            var tagList = (tags ?? Enumerable.Empty<string>()).ToList();
            tagList.Add(protocolConfiguration.Protocol);
            tagList.Add(protocolConfiguration.Transmit);
            tagList.Add(versionLabel);

            var instance = new RegistryInformation
            {
                Name = serviceName,
                Id = serviceId,
                Address = await DnsUtil.GetIpAddressAsync(),
                Port = serviceConfiguration.Port,
                Version = serviceConfiguration.Version,
                Tags = tagList
            };
            ServiceInstances.Add(instance);
            return instance;
        }

        public Task DeregisterServiceAsync()
        {
            return Task.Run(() => ServiceInstances.Clear());
        }

        public Task<bool> DeregisterServiceAsync(string serviceId)
        {
            if (ServiceInstances.Exists(x => x.Id == serviceId))
            {
                ServiceInstances.RemoveAt(ServiceInstances.FindIndex(x => x.Id == serviceId));
            }
            return Task.FromResult(true);
        }
    }
}
