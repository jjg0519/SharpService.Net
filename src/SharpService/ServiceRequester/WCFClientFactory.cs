using System.ServiceModel;
using ProtoBuf.ServiceModel;
using SharpService.Configuration;
using SharpService.ServiceDiscovery;
using SharpService.Components;
using System.Threading.Tasks;
using SharpService.LoadBalance;

namespace SharpService.ServiceRequester
{
    public class WCFClientFactory
    {
        public static async Task<RegistryInformation> GetService(
            RefererConfiguration refererConfig)
        {
            var serviceDiscoveryProvider = await ObjectContainer.Resolve<IServiceDiscoveryProviderFactory>().GetAsync();
            var serviceName = await serviceDiscoveryProvider.GetServiceName(
                refererConfig.Interface,
                refererConfig.Assembly);
            var services = await serviceDiscoveryProvider.FindServicesAsync(serviceName);
            var loadBalance = await ObjectContainer.Resolve<ILoadBalanceFactory>().GetAsync(refererConfig);
            loadBalance.OnRefresh(services);
            var service = loadBalance.Select();
            return service;
        }

        public static ChannelFactory<Interface> CreateChannelFactory<Interface>(
          RegistryInformation service)
        {
            var binding = ConfigurationHelper.CreateBinding(service.Tags[0], (SecurityMode)(int.Parse(service.Tags[1])));
            var endpoint = new EndpointAddress(service.Address);
            var factory = new ChannelFactory<Interface>(binding, endpoint);
            factory.Endpoint.Behaviors.Add(new ProtoEndpointBehavior());
            return factory;
        }
    }
}
