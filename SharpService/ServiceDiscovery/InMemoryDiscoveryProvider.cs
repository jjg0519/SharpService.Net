using SemVer;
using SharpService.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace SharpService.ServiceDiscovery
{
    public class InMemoryDiscoveryProvider : ServiceDiscoveryProvider
    {
        private const string serviceConfig = "serviceGroup/serviceConfig";
        private readonly List<ServiceConfiguration> serviceConfigurations = ConfigurationManager.GetSection(serviceConfig) as List<ServiceConfiguration>;
     
        private readonly List<RegistryInformation> _serviceInstances = new List<RegistryInformation>();

        public IList<RegistryInformation> ServiceInstances
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

        public override async Task<RegistryInformation> RegisterServiceAsync(ServiceConfiguration serviceConfig)
        {
            var serviceName = await GetServiceName(serviceConfig.Interface, serviceConfig.Address);
            var version = serviceConfig.Version;
            var uri = new Uri(serviceConfig.Address);
            var tags = new List<string>() { serviceConfig.Binding, serviceConfig.Security.ToString() };
            return await RegisterServiceAsync(serviceName, version, uri, tags);
        }

        public override Task<RegistryInformation> RegisterServiceAsync(string serviceName, string version, Uri uri, IEnumerable<string> tags = null)
        {
            string versionLabel = $"{VERSION_PREFIX}{version}";
            var tagList = (tags ?? Enumerable.Empty<string>()).ToList();
            tagList.Add(versionLabel);

            var registryInformation = new RegistryInformation
            {
                Name = serviceName,
                Id = Guid.NewGuid().ToString(),
                Address = uri.Host,
                Port = uri.Port,
                Version = version,
                Tags = tags ?? Enumerable.Empty<string>()
            };
            _serviceInstances.Add(registryInformation);
            return Task.FromResult(registryInformation);
        }

        public override async Task<bool> DeregisterServiceAsync(string serviceId)
        {
            var instance = (await FindServicesAsync()).FirstOrDefault(x => x.Id == serviceId);
            if (instance != null)
            {
                ServiceInstances.Remove(instance);
                return true;
            }
            return false;
        }

        public override Task<IList<RegistryInformation>> FindServicesAsync()
        {
            return Task.FromResult(ServiceInstances);
        }

        public override async Task<IList<RegistryInformation>> FindServicesAsync(string name)
        {
            var instances = await FindServicesAsync();
            return instances.Where(x => x.Name == name).ToList();
        }

        public override async Task<IList<RegistryInformation>> FindServicesWithVersionAsync(string name, string version)
        {
            var instances = await FindServicesAsync(name);
            var range = new Range(version);
            return instances.Where(x => range.IsSatisfied(x.Version)).ToList();
        }

        public override Task<IList<RegistryInformation>> FindAllServicesAsync()
        {
            return Task.FromResult(ServiceInstances);
        }

    }
}
