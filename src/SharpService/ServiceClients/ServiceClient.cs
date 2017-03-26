using SharpService.Components;
using SharpService.Configuration;
using SharpService.LoadBalance;
using SharpService.ServiceDiscovery;
using System.Linq;
using System.Threading.Tasks;

namespace SharpService.ServiceClients
{
    public class ServiceClient
    {
        private IConfigurationObject configuration { get; set; }

        private IServiceClientProviderFactory serviceClientProviderFactory { get; set; }

        private IServiceDiscoveryProviderFactory serviceDiscoveryProviderFactory { get; set; }

        private ILoadBalanceFactory loadBalanceFactory { get; set; }

        public ServiceClient()
        {
            configuration = ObjectContainer.Resolve<IConfigurationObject>();
            serviceClientProviderFactory = ObjectContainer.Resolve<IServiceClientProviderFactory>();
            serviceDiscoveryProviderFactory = ObjectContainer.Resolve<IServiceDiscoveryProviderFactory>();
            loadBalanceFactory = ObjectContainer.Resolve<ILoadBalanceFactory>();
        }

        private IServiceClientProvider serviceClientProvider { get; set; }

        private IServiceDiscoveryProvider serviceDiscoveryProvider { get; set; }

        private ILoadBalanceProvider loadBalanceProvider { get; set; }

        private RefererConfiguration refererConfiguration { get; set; }

        private ProtocolConfiguration protocolConfiguration { get; set; }

        private string serviceName { get; set; }

        private void BuildConfiguration(string refererId)
        {
            refererConfiguration = configuration.refererConfigurations.Where(x => x.Id == refererId).FirstOrDefault();
            protocolConfiguration = string.IsNullOrEmpty(refererConfiguration.Protocol) ?
                 configuration.protocolConfigurations.Where(x => x.Defalut).FirstOrDefault() :
                 configuration.protocolConfigurations.Where(x => x.Name == refererConfiguration.Protocol).FirstOrDefault();
            serviceClientProvider = serviceClientProviderFactory.Get(protocolConfiguration.Protocol);
            serviceDiscoveryProvider = serviceDiscoveryProviderFactory.Get();
            loadBalanceProvider = loadBalanceFactory.Get(protocolConfiguration.LoadBalance);
            serviceName = ServiceDiscoveryHelper.GetServiceName(protocolConfiguration.Protocol, refererConfiguration.Interface);
        }

        public IService GetServiceClient<IService>(string refererId)
        {
            BuildConfiguration(refererId);
            var registryInformations = serviceDiscoveryProvider.FindServicesAsync(serviceName).GetAwaiter().GetResult();
            var registryInformation = loadBalanceProvider.Select(registryInformations);
            return serviceClientProvider.GetServiceClient<IService>(registryInformation, protocolConfiguration);
        }

        public async Task<IService> GetServiceClientAsync<IService>(string refererId)
        {
            BuildConfiguration(refererId);      
            var registryInformations = await serviceDiscoveryProvider.FindServicesAsync(serviceName);
            var registryInformation = loadBalanceProvider.Select(registryInformations);
            return serviceClientProvider.GetServiceClient<IService>(registryInformation, protocolConfiguration);
        }
    }
}
