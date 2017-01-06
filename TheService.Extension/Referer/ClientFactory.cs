using TheService.Extension.ConfigFactory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TheService.Extension.Behavior;

namespace TheService.Extension.Referer
{
    public class ClientFactory
    {
        private static string refererConfig = "serviceGroup/refererConfig";
        private static List<RefererElement> referers = ConfigurationManager.GetSection(refererConfig) as List<RefererElement>;

        public static ChannelFactory<IObjcet> CreateChannelFactory<IObjcet>(string id, Dictionary<string, string> messages = null)
        {
            var referer = referers.FirstOrDefault(x => x.Id == id);
            if (referer == null)
            {
                throw new Exception("can not find referer config");
            }
            var binding = ConfigHelper.CreateBinding(referer.Binding, (SecurityMode)referer.Security);
            var endpoint = new EndpointAddress(referer.Addresss[0]);
            var factory = new ChannelFactory<IObjcet>(binding, endpoint);
            factory.Endpoint.Behaviors.Add(new MyEndpointBehavior(messages));
            return factory;
        }
    }
}
