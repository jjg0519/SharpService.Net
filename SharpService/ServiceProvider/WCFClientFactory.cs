using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using ProtoBuf.ServiceModel;
using SharpService.Configuration;

namespace SharpService.Factory
{
    public class WCFClientFactory
    {
        private const string refererConfig = "serviceGroup/refererConfig";
        private static List<RefererElement> refererElements = ConfigurationManager.GetSection(refererConfig) as List<RefererElement>;

        public static ChannelFactory<Interface> CreateChannelFactory<Interface>(string id)
        {
            var refererElement = refererElements.FirstOrDefault(x => x.Id == id);
            if (refererElement == null)
            {
                throw new ArgumentNullException("can not find referer config");
            }
            var binding = ConfigurationHelper.CreateBinding(refererElement.Referers[0].Binding, (SecurityMode)refererElement.Referers[0].Security);
            var endpoint = new EndpointAddress(refererElement.Referers[0].Address);
            var factory = new ChannelFactory<Interface>(binding, endpoint);
            factory.Endpoint.Behaviors.Add(new ProtoEndpointBehavior());
            return factory;
        }
    }
}
