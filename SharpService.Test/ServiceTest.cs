using NUnit.Framework;
using ServiceTestLib;
using System;
using SharpService.Requester;
using Polly.Timeout;
using System.Threading;
using SharpService.DependencyInjection;
using SharpService.Components;

namespace SharpService.Test
{
    public class ServiceTest
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            ConfigurationBuilder
                 .Create()
                 .UseAutofac()
                 .UseExceptionlessLogger()
                 .UseDelegatingHandler()
                 .UseIPollyCircuitBreakingDelegatingHandler(
                    exceptionsAllowedBeforeBreaking: 2,
                    durationOfBreak: TimeSpan.FromSeconds(60),
                    timeoutValue: TimeSpan.FromSeconds(30),
                    timeoutStrategy: TimeoutStrategy.Pessimistic
                )
                 .UseWCFServiceProvider()
                 .Build();

            ObjectContainer.Resolve<ServiceProvider.IServiceProvider>().Provider();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            ObjectContainer.Resolve<ServiceProvider.IServiceProvider>().Close();
        }

        [Test]
        public void TestSendMessage()
        {
            var client = new RequestProxy()
                .UseId("helloService")
                .BuilderClient<IHelloService>();
            Console.WriteLine(client.SendMessage("SendMessage"));

        }

        [Test]
        public void TestSendData()
        {
            var client = new RequestProxy()
               .UseId("helloService")
               .BuilderClient<IHelloService>();
            Console.WriteLine(client.SendData(new Data { Message = "SendData" }).Message);
        }

        [Test]
        public void TestPollyCircuitBreakingDelegatingHandler()
        {
            var proxy = new RequestProxy()
               .ConfigurerRequester(
                new PollyCircuitBreakingDelegatingHandler(
                    exceptionsAllowedBeforeBreaking: 2,
                    durationOfBreak: TimeSpan.FromSeconds(60),
                    timeoutValue: TimeSpan.FromSeconds(30),
                    timeoutStrategy: TimeoutStrategy.Pessimistic
                    )
                );
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
