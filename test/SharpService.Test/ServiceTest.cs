using NUnit.Framework;
using ServiceTestLib;
using System;
using System.Threading;
using SharpService.DependencyInjection;
using System.Threading.Tasks;
using SharpService.ServiceProviders;
using SharpService.ServiceClients;

namespace SharpService.Test
{
    public class ServiceTest
    {
        [OneTimeSetUp]
        public async Task SetUp()
        {
            ConfigurationBuilder
                 .Create()
                 .UseAutofac()
                 .UseConfigurationObject()
                 .UseLog4Net()
                 .UseServiceProvider()
                 .UseServiceDiscoveryProvider()
                 .UseServiceClientProvider()
                 .UseLoadBalance();

            await new ServiceProvider().Provider();

        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await new ServiceProvider().Close();
        }

        [Test]
        public void TestSendMessage()
        {
            var client = new ServiceClient().GetServiceClient<IHelloService>("helloService");
            Console.WriteLine(client.SendMessage("SendMessage"));
        }

        [Test]
        public void TestSendData()
        {
            var client = new ServiceClient().GetServiceClient<IHelloService>("helloService");
            Console.WriteLine(client.SendData(new Data { Message = "SendData" }).Message);
        }

        [Test]
        public void TestPollyCircuitBreaking()
        {
            var client = new ServiceClient().GetServiceClient<IHelloService>("helloService");
            try { client.Error(); } catch { }
            try { client.Error(); } catch { }
            try { client.OK(); } catch { }
            Thread.Sleep(TimeSpan.FromSeconds(60));
            try { client.OK(); } catch { }
        }
    }
}
