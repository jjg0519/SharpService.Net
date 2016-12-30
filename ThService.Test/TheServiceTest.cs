using NUnit.Framework;
using ServiceTestLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using TheService.Extension;
using TheService.Extension.ConfigFactory;

namespace ThService.Test
{
    public class TheServiceTest
    {
        [Test]
        public void TestConsumerConfig()
        {
            string consumerConfig = "serviceGroup/consumerConfig";
            List<ConsumerElement> consumers = ConfigurationManager.GetSection(consumerConfig) as List<ConsumerElement>;
        }

        [Test]
        public void TestHelloService()
        {
            ServiceFactory.Start();
            var client = (IHelloService)new ServiceRealProxy<IHelloService>("helloService", new Dictionary<string, string>() { { "test", "sdfsfdsdfsfd" } }).GetTransparentProxy();
            Console.WriteLine(client.GetMessage("test"));
            ServiceFactory.Stop();
        }
    }
}
