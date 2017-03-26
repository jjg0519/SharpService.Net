using SharpService.DependencyInjection;
using SharpService.ServiceProviders;
using System;

namespace Sample.ServiceProviderHost
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Sample.ServiceProvider";
            ConfigurationBuilder
                .Create()
                .UseAutofac()
                .UseConfigurationObject()
                .UseLog4Net()
                .UseServiceProvider()
                .UseServiceDiscoveryProvider();

            new ServiceProvider().Provider().GetAwaiter();
            Console.ReadKey();
        }
    }
}
