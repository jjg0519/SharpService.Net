using NUnit.Framework;
using ServiceTestLib;
using System;
using SharpService.Factory;
using SharpService.Requester;
using Polly.Timeout;
using System.Threading;
using SharpService.Logging;

namespace SharpService.Test
{
    public class CallServiceTest
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            new WCFServiceFactory().ProviderService().RegistryService();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            new WCFServiceFactory().CloseService().CancelService();
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
                    timeoutStrategy: TimeoutStrategy.Pessimistic,
                    logger: new ExceptionlessLogger()
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
