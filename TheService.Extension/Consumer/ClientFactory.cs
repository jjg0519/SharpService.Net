using TheService.Extension.ConfigFactory;
using TheService.Extension.Message;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace TheService.Extension.Client
{
    public class ClientFactory
    {
        private static string consumerConfig = "serviceGroup/consumerConfig";
        private static List<ConsumerElement> consumers = ConfigurationManager.GetSection(consumerConfig) as List<ConsumerElement>;

        public static ChannelFactory<IObjcet> CreateChannelFactory<IObjcet>(string id, Dictionary<string, string> messages = null)
        {
            var consumer = consumers.FirstOrDefault(x => x.Id == id);
            if (consumer == null)
            {
                throw new Exception("can not find consumer config");
            }
            var binding = ConfigHelper.CreateBinding(consumer.Binding, (SecurityMode)consumer.Security);
            var endpoint = new EndpointAddress(consumer.Addresss[0]);
            var client = new ChannelFactory<IObjcet>(binding, endpoint);
            client.Endpoint.Behaviors.Add(new MessageEndpointBehavior(messages));
            return client;
        }
    }
}
