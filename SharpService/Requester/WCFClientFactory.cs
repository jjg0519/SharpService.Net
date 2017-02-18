using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using ProtoBuf.ServiceModel;
using SharpService.Configuration;
using SharpService.ServiceDiscovery;
using SharpService.Components;
using System.Threading.Tasks;

namespace SharpService.Requester
{
    public class WCFClientFactory
    {
        private const string refererConfig = "serviceGroup/refererConfig";
        private static List<RefererConfiguration> refererConfigurations = ConfigurationManager.GetSection(refererConfig) as List<RefererConfiguration>;

        public static async Task<ChannelFactory<Interface>> CreateChannelFactory<Interface>(string id)
        {
            var refererConfiguration = refererConfigurations.FirstOrDefault(x => x.Id == id);
            if (refererConfiguration == null)
            {
                throw new ArgumentNullException("can not find referer config");
            }
            var serviceDiscoveryProvider = await ObjectContainer.Resolve<IServiceDiscoveryProviderFactory>().GetAsync();
            var serviceName = await serviceDiscoveryProvider.GetServiceName(
                refererConfiguration.Interface,
                refererConfiguration.Assembly);
            var services = await serviceDiscoveryProvider.FindServicesAsync(serviceName);
            var service = services[0];
            var binding = ConfigurationHelper.CreateBinding(service.Tags[0], (SecurityMode)(int.Parse(service.Tags[1])));
            var address = ConfigurationHelper.CreateAddress(service.Tags[2], service.Tags[0]);
            var endpoint = new EndpointAddress(address);
            var factory = new ChannelFactory<Interface>(binding, endpoint);
            factory.Endpoint.Behaviors.Add(new ProtoEndpointBehavior());
            return factory;
        }
    }
}
