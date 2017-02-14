using TheService.Extension.ConfigFactory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using ProtoBuf.ServiceModel;
using TheService.Extension.Behavior;

namespace TheService.Extension.Client
{
    public class ClientFactory
    {
        private static string refererConfig = "serviceGroup/refererConfig";
        private static List<RefererElement> refererElements = ConfigurationManager.GetSection(refererConfig) as List<RefererElement>;

        public static ChannelFactory<IObjcet> CreateChannelFactory<IObjcet>(string id, Dictionary<string, string> messages = null)
        {
            var refererElement = refererElements.FirstOrDefault(x => x.Id == id);
            if (refererElement == null)
            {
                throw new Exception("can not find referer config");
            }
            var binding = ConfigHelper.CreateBinding(refererElement.Referers[0].Binding, (SecurityMode)refererElement.Referers[0].Security);
            var endpoint = new EndpointAddress(refererElement.Referers[0].Address);
            var factory = new ChannelFactory<IObjcet>(binding, endpoint);
            factory.Endpoint.Behaviors.Add(new AttachMessageBehavior(messages));
            return factory;
        }
    }
}
