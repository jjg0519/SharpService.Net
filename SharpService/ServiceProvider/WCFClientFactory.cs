using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using ProtoBuf.ServiceModel;
using SharpService.Configuration;

namespace SharpService.ServiceProvider
{
    public class WCFClientFactory
    {
        private const string refererConfig = "serviceGroup/refererConfig";
        private static List<RefererConfiguration> refererConfigurations = ConfigurationManager.GetSection(refererConfig) as List<RefererConfiguration>;

        public static ChannelFactory<Interface> CreateChannelFactory<Interface>(string id)
        {
            var refererConfiguration = refererConfigurations.FirstOrDefault(x => x.Id == id);
            if (refererConfiguration == null)
            {
                throw new ArgumentNullException("can not find referer config");
            }
            var binding = ConfigurationHelper.CreateBinding(refererConfiguration.Referers[0].Binding, (SecurityMode)refererConfiguration.Referers[0].Security);
            var endpoint = new EndpointAddress(refererConfiguration.Referers[0].Address);
            var factory = new ChannelFactory<Interface>(binding, endpoint);
            factory.Endpoint.Behaviors.Add(new ProtoEndpointBehavior());
            return factory;
        }
    }
}
