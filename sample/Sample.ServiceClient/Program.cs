using Sample.Service;
using SharpService.DependencyInjection;
using SharpService.ServiceClients;
using System;
using System.Collections.Generic;

namespace Sample.ServiceClientHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Sample.ServiceClient";
            ConfigurationBuilder
                .Create()
                .UseAutofac()
                .UseConfigurationObject()
                .UseLog4Net()
                .UseServiceClientProvider()
                .UseLoadBalance();

            var client = new ServiceClient().GetServiceClient<ISampleService>("sampleService");

            Console.WriteLine($"TestArgVoid-{client.TestArgVoid()}") ;
            Console.WriteLine($"TestBoolean-{client.TestBoolean(true).ToString()}");
            Console.WriteLine($"TestDouble-{client.TestDouble(0.1).ToString()}");
            Console.WriteLine($"TestFloat-{client.TestFloat((float)0.1).ToString()}");
            Console.WriteLine($"TestInt-{client.TestInt(1).ToString()}");
            Console.WriteLine($"TestList-{string.Join(",", client.TestList(new List<string>() { "list" })) }");
            Console.WriteLine($"TestLong-{client.TestLong(1).ToString()}");
            Console.WriteLine($"TestMap-{string.Join(",", client.TestMap(new Dictionary<string, string>() { { "key", "value" } }).Keys)}");
            Console.WriteLine($"TestString-{ client.TestString("TestString")}");
            Console.WriteLine($"TestVoid-"); client.TestVoid();
            Console.ReadKey();
        }
    }
}
