using Sample.Service;
using SharpService.Components;
using SharpService.DependencyInjection;
using SharpService.ServiceDiscovery;
using SharpService.ServiceRequester;
using System;
using System.Collections.Generic;

namespace Sample.ServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Sample.ServiceClient";
            ConfigurationBuilder
                .Create()
                .UseAutofac()
                .UseExceptionlessLogger()
                .UseWCFDelegatingHandler()
                .UseServiceDiscoveryProvider()
                .UseLoadBalance();

            var client = new RequestClient()
                 .UseId("sampleService")
                 .UseDelegatingHandler<WCFDelegatingHandler>()
                 .BuilderClient<ISampleService>();

            Console.WriteLine($"TestArgVoid-{client.TestArgVoid()}") ;
            Console.WriteLine($"TestBoolean-{client.TestBoolean(true).ToString()}");
            Console.WriteLine($"TestDouble-{client.TestDouble(0.1).ToString()}");
            Console.WriteLine($"TestFloat-{client.TestFloat((float)0.1).ToString()}");
            Console.WriteLine($"TestInt-{client.TestInt(1).ToString()}");
            Console.WriteLine($"TestList-{string.Join(",", client.TestList(new List<object>() { "list" })) }");
            Console.WriteLine($"TestLong-{client.TestLong(1).ToString()}");
            Console.WriteLine($"TestMap-{string.Join(",", client.TestMap(new Dictionary<string, object>() { { "key", "value" } }).Keys)}");
            Console.WriteLine($"TestString-{ client.TestString("TestString")}");
            Console.WriteLine($"TestVoid-"); client.TestVoid();
            Console.ReadKey();
        }
    }
}
