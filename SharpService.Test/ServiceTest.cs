using NUnit.Framework;
using ServiceTestLib;
using System;
using SharpService.Requester;
using Polly.Timeout;
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
                 .UseExceptionlessLogger()
                 .UseDelegatingHandler()
                 .UsePollyCircuitBreakingDelegatingHandler(
                    exceptionsAllowedBeforeBreaking: 2,
                    durationOfBreak: TimeSpan.FromSeconds(60),
                    timeoutValue: TimeSpan.FromSeconds(30),
                    timeoutStrategy: TimeoutStrategy.Pessimistic
                )
                .UseWCFServiceProvider()
                .UseServiceDiscoveryProvider();

            ObjectContainer.Resolve<ServiceProvider.IServiceProvider>().Provider();
            var serviceDiscoveryProvider = await ObjectContainer.Resolve<IServiceDiscoveryProviderFactory>()
                .GetAsync();
            await serviceDiscoveryProvider.RegisterServiceAsync();
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            ObjectContainer.Resolve<ServiceProvider.IServiceProvider>().Close();
            var serviceDiscoveryProvider = await ObjectContainer.Resolve<IServiceDiscoveryProviderFactory>()
               .GetAsync();
            await serviceDiscoveryProvider.DeregisterServiceAsync();
        }

        [Test]
        public void TestSendMessage()
        {
            var client = new RequestClient()
                .UseId("helloService")
                .UseRequesterHandler<DelegatingHandler>()
                .BuilderClient<IHelloService>();
            Console.WriteLine(client.SendMessage("SendMessage"));
        }

        [Test]
        public void TestSendData()
        {
            var client = new RequestClient()
                 .UseId("helloService")
                .UseRequesterHandler<DelegatingHandler>()
                .BuilderClient<IHelloService>();
            Console.WriteLine(client.SendData(new Data { Message = "SendData" }).Message);
        }

        [Test]
        public void TestPollyCircuitBreakingDelegatingHandler()
        {
            var proxy = new RequestClient()
               .UseRequesterHandler<PollyCircuitBreakingDelegatingHandler>();
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
