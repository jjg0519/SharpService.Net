using NUnit.Framework;
using ServiceTestLib;
using System;
using SharpService.ServiceRequester;
using System.Threading;
using SharpService.DependencyInjection;
using SharpService.Components;
using SharpService.ServiceDiscovery;
using System.Threading.Tasks;

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
                 .UseExceptionlessLogger()
                 .UseWCFDelegatingHandler()
                 .UsePollyCircuitBreakingDelegatingHandler(
                    exceptionsAllowedBeforeBreaking: 2,
                    durationOfBreak: TimeSpan.FromSeconds(60)
                )
                .UseServiceProvider()
                .UseServiceDiscoveryProvider()
                .UseLoadBalance();

            ObjectContainer.Resolve<ServiceProvider.IServiceProvider>().Provider();
            var serviceDiscoveryProvider = ObjectContainer.Resolve<IServiceDiscoveryProviderFactory>().Get();
            await serviceDiscoveryProvider.RegisterServiceAsync();
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            ObjectContainer.Resolve<ServiceProvider.IServiceProvider>().Close();
            var serviceDiscoveryProvider = ObjectContainer.Resolve<IServiceDiscoveryProviderFactory>().Get();
            await serviceDiscoveryProvider.DeregisterServiceAsync();
        }

        [Test]
        public void TestSendMessage()
        {
            var client = new RequestClient()
                .UseId("helloService")
                .UseDelegatingHandler<WCFDelegatingHandler>()
                .BuilderClient<IHelloService>();
            Console.WriteLine(client.SendMessage("SendMessage"));
        }

        [Test]
        public void TestSendData()
        {
            var client = new RequestClient()
                 .UseId("helloService")
                .UseDelegatingHandler<WCFDelegatingHandler>()
                .BuilderClient<IHelloService>();
            Console.WriteLine(client.SendData(new Data { Message = "SendData" }).Message);
        }

        [Test]
        public void TestPollyCircuitBreakingDelegatingHandler()
        {
            var proxy = new RequestClient()
               .UseDelegatingHandler<PollyCircuitBreakingDelegatingHandler>();
            var client = proxy.UseId("helloService").BuilderClient<IHelloService>();
            try { client.Error(); } catch { }
            try { client.Error(); } catch { }
            try { client.OK(); } catch { }
            Thread.Sleep(TimeSpan.FromSeconds(60));
            try { client.OK(); } catch { }
            try { client.TimeOut(); } catch { }
        }
    }
}
