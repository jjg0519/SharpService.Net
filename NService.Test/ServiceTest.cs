using NUnit.Framework;
using ServiceTestLib;
using System;
using NService.Factory;
using NService.Requester;
using Polly.Timeout;
using System.Threading;

namespace NService.Test
{
    public class ServiceTest
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            ServiceFactory.Provider();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            ServiceFactory.Cancel();
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
            Console.WriteLine(client.SendData(new Data { Name = "SendData" }).Name);
        }

        [Test]
        public void TestPollyCircuitBreakingDelegatingHandler()
        {
            var proxy = new RequestProxy()
               .ConfigurerRequester(
                new PollyCircuitBreakingDelegatingHandler(
                    exceptionsAllowedBeforeBreaking: 2,
                    durationOfBreak: TimeSpan.FromMinutes(1),
                    timeoutValue: TimeSpan.FromMinutes(1),
                    timeoutStrategy: TimeoutStrategy.Optimistic
                    )
                );
            var client = proxy.UseId("helloService").BuilderClient<IHelloService>();
            try { client.Error(); } catch { }
            try { client.Error(); } catch { }
            try { client.OK(); } catch { }
            Thread.Sleep(TimeSpan.FromMinutes(1));
            try { client.OK(); } catch { }
            try { client.TimeOut(); } catch { }
        }
    }
}
