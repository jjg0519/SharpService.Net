using NUnit.Framework;
using ServiceTestLib;
using System;
using NService.Factory;
using NService.Requester;

namespace NService.Test
{
    public class ServiceTest
    {
        [Test]
        public void TestHelloService()
        {
            ServiceFactory.Provider();
            var proxy = new RequestProxy()
                .UseId("helloService")
                //.ConfigurerRequester(new PollyCircuitBreakingDelegatingHandler())
                .BuilderProxy<IHelloService>();
            Console.WriteLine(proxy.GetMessage("Test"));
            Console.WriteLine(proxy.GetPerson(new Person { Name = "TestName" }).Name);
            ServiceFactory.Stop();
        }
    }
}
