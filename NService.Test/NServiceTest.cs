using NUnit.Framework;
using ServiceTestLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using NService.Configuration;
using NService.Factory;
using NService.Requester;

namespace NService.Test
{
    public class NServiceTest
    {
        [Test]
        public void TestRefererConfig()
        {
            string refererConfig = "serviceGroup/refererConfig";
            List<RefererElement> refererElements = ConfigurationManager.GetSection(refererConfig) as List<RefererElement>;
        }

        [Test]
        public void TestHelloService()
        {
            ServiceFactory.Start();
            var proxy = new RequestProxy().UseId("helloService").Builder<IHelloService>();
            Console.WriteLine(proxy.GetMessage("test"));
            Console.WriteLine(proxy.GetMessage1(new Person { PerName = "testName" }).PerName);
            ServiceFactory.Stop();
        }
    }
}
