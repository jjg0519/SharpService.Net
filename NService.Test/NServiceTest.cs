﻿using NUnit.Framework;
using ServiceTestLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using NService.Core;
using NService.Core.Client;
using NService.Core.ConfigFactory;


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
            var client = TheProxy.TheRequestProxy<IHelloService>("helloService");
            Console.WriteLine(client.GetMessage("test"));
            Console.WriteLine(client.GetMessage1(new Person { PerName = "testName" }).PerName);
            ServiceFactory.Stop();
        }
    }
}