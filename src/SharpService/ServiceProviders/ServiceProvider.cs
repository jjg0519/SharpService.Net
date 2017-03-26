using SharpService.Components;
using SharpService.Configuration;
using SharpService.ServiceDiscovery;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpService.ServiceProviders
{
    public class ServiceProvider
    {
        private IServiceDiscoveryProvider serviceDiscoveryProvider { set; get; }
        private IServiceProviderFactory serviceProviderFactory { set; get; }
        private IConfigurationObject configuration { get; set; }

        public ServiceProvider()
        {
            serviceDiscoveryProvider = ObjectContainer.Resolve<IServiceDiscoveryProviderFactory>().Get();
            serviceProviderFactory = ObjectContainer.Resolve<IServiceProviderFactory>();
            configuration = ObjectContainer.Resolve<IConfigurationObject>();
        }

        public Dictionary<string, List<ServiceConfiguration>> GetProvider()
        {
            var providers = new Dictionary<string, List<ServiceConfiguration>>();
            foreach (var protocolConfiguration in configuration.protocolConfigurations)
            {
                var serviceConfigurations = protocolConfiguration.Defalut == true ?
                    configuration.serviceConfigurations.Where(x => string.IsNullOrEmpty(x.Protocol)).ToList() :
                    configuration.serviceConfigurations.Where(x => x.Protocol == protocolConfiguration.Name).ToList();
                if (providers.ContainsKey(protocolConfiguration.Protocol))
                {
                    providers[protocolConfiguration.Protocol].AddRange(serviceConfigurations);
                }
                else
                {
                    providers.Add(protocolConfiguration.Protocol, new List<ServiceConfiguration>(serviceConfigurations));
                }
            }
            return providers;
        }

        public async Task Provider()
        {
            var providers = GetProvider();
            foreach (var protocol in providers.Keys)
            {
                var serviceProvider = serviceProviderFactory.Get(protocol);
                var serviceConfigurations = providers[protocol];
                serviceProvider.Provider(serviceConfigurations);
            }
            await serviceDiscoveryProvider.RegisterServiceAsync();
        }

        public async Task Close()
        {
            foreach (var protocol in configuration.protocolConfigurations.Select(s => s.Protocol))
            {
                var serviceProvider = serviceProviderFactory.Get(protocol);
                serviceProvider.Close();
            }
            await serviceDiscoveryProvider.DeregisterServiceAsync();
        }
    }
}
