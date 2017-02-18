using Autofac;
using Polly.Timeout;
using SharpService.Components;
using SharpService.Logging;
using SharpService.Requester;
using SharpService.ServiceDiscovery;
using SharpService.ServiceProvider;
using System;

namespace SharpService.DependencyInjection
{
    public static class ConfigurationBuilderExtensions
    {
        public static ConfigurationBuilder UseAutofac(this ConfigurationBuilder configuration)
        {
            return UseAutofac(configuration, new ContainerBuilder());
        }

        public static ConfigurationBuilder UseAutofac(this ConfigurationBuilder configuration, ContainerBuilder containerBuilder)
        {
            ObjectContainer.SetContainer(new AutofacObjectContainer(containerBuilder));
            return configuration;
        }

        public static ConfigurationBuilder UseDelegatingHandler(this ConfigurationBuilder configuration)
        {
            configuration.SetDefault<IRequesterHandler, DelegatingHandler>(typeof(DelegatingHandler).FullName);
            return configuration;
        }

        public static ConfigurationBuilder UsePollyCircuitBreakingDelegatingHandler(this ConfigurationBuilder configuration, 
            int exceptionsAllowedBeforeBreaking,
            TimeSpan durationOfBreak,
            TimeSpan timeoutValue,
            TimeoutStrategy timeoutStrategy)
        {
            configuration.SetDefault<IRequesterHandler, PollyCircuitBreakingDelegatingHandler>(
                new PollyCircuitBreakingDelegatingHandler(
                    exceptionsAllowedBeforeBreaking: exceptionsAllowedBeforeBreaking,
                    durationOfBreak: durationOfBreak,
                    timeoutValue: timeoutValue,
                    timeoutStrategy: timeoutStrategy
                    ),
                typeof(PollyCircuitBreakingDelegatingHandler).FullName);
            return configuration;
        }

        public static ConfigurationBuilder UseWCFServiceProvider(this ConfigurationBuilder configuration)
        {
            configuration.SetDefault<ServiceProvider.IServiceProvider, WCFServiceProvider>();
            return configuration;
        }

        public static ConfigurationBuilder UseServiceDiscoveryProvider(this ConfigurationBuilder configuration)
        {
            UseServiceDiscoveryProviderFactory(configuration);
            UseInMemoryDiscoveryProvider(configuration);
            UseConsulDiscoveryProvider(configuration);
            UseZooKeeperDiscoveryProviderr(configuration);
            return configuration;
        }

        public static ConfigurationBuilder UseServiceDiscoveryProviderFactory(this ConfigurationBuilder configuration)
        {
            configuration.SetDefault<IServiceDiscoveryProviderFactory, ServiceDiscoveryProviderFactory>();
            return configuration;
        }

        public static ConfigurationBuilder UseInMemoryDiscoveryProvider(this ConfigurationBuilder configuration)
        {
            configuration.SetDefault<IServiceDiscoveryProvider, InMemoryDiscoveryProvider>(
                typeof(InMemoryDiscoveryProvider).FullName);
            return configuration;
        }

        public static ConfigurationBuilder UseConsulDiscoveryProvider(this ConfigurationBuilder configuration)
        {
            configuration.SetDefault<IServiceDiscoveryProvider, ConsulDiscoveryProvider>(typeof(ConsulDiscoveryProvider).FullName);
            return configuration;
        }

        public static ConfigurationBuilder UseZooKeeperDiscoveryProviderr(this ConfigurationBuilder configuration)
        {
            configuration.SetDefault<IServiceDiscoveryProvider, ZooKeeperDiscoveryProvider>(typeof(ZooKeeperDiscoveryProvider).FullName);
            return configuration;
        }

        public static ConfigurationBuilder UseExceptionlessLogger(this ConfigurationBuilder configuration)
        {
            configuration.SetDefault<ISharpServiceLogger, ExceptionlessLogger>();
            return configuration;
        }
    }
}