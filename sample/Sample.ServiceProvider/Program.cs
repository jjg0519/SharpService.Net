using SharpService.Components;
using SharpService.DependencyInjection;
using SharpService.ServiceDiscovery;
using System;

namespace Sample.ServiceProvider
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Sample.ServiceProvider";
            ConfigurationBuilder
                .Create()
                .UseAutofac()
                .UseExceptionlessLogger()
               .UseWCFServiceProvider()
               .UseServiceDiscoveryProvider();

            ObjectContainer.Resolve<SharpService.ServiceProvider.IServiceProvider>().Provider();
            var serviceDiscoveryProvider = ObjectContainer.Resolve<IServiceDiscoveryProviderFactory>().GetAsync().Result;
            serviceDiscoveryProvider.RegisterServiceAsync();
            Console.ReadKey();
        }
    }
}
