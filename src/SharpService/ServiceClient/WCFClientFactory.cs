using System.ServiceModel;
using ProtoBuf.ServiceModel;
using SharpService.Configuration;
using SharpService.ServiceDiscovery;
using SharpService.Components;
using System.Threading.Tasks;
using SharpService.LoadBalance;
using SharpService.WCF;

namespace SharpService.ServiceRequester
{
    public class WCFClientFactory
    {
        public static async Task<RegistryInformation> GetService(
            RefererConfiguration refererConfig)
        {
            var serviceDiscoveryProvider = ObjectContainer.Resolve<IServiceDiscoveryProviderFactory>().Get();
            var serviceName = await serviceDiscoveryProvider.GetServiceNameAsync(refererConfig.Interface, refererConfig.Assembly);
            var services = await serviceDiscoveryProvider.FindServicesAsync(serviceName);
            var loadBalance = ObjectContainer.Resolve<ILoadBalanceFactory>().Get(null);
            loadBalance.OnRefresh(services);
            var service = loadBalance.Select();
            return service;
        }

        public static ChannelFactory<Interface> CreateChannelFactory<Interface>(
          RegistryInformation service)
        {
            var binding = WCFHelper.CreateBinding(service.Tags[0]);
            var endpoint = new EndpointAddress(service.Address);
            var factory = new ChannelFactory<Interface>(binding, endpoint);
            factory.Endpoint.Behaviors.Add(new ProtoEndpointBehavior());
            return factory;
        }
    }
}
