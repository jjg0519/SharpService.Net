using Autofac;
using Polly.Timeout;
using SharpService.Components;
using SharpService.LoadBalance;
using SharpService.Logging;
using SharpService.ServiceRequester;
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

        public static ConfigurationBuilder UseWCFDelegatingHandler(this ConfigurationBuilder configuration)
        {
            configuration.SetDefault<IDelegatingHandler, WCFDelegatingHandler>(typeof(WCFDelegatingHandler).FullName);
            return configuration;
        }

        public static ConfigurationBuilder UsePollyCircuitBreakingDelegatingHandler(
            this ConfigurationBuilder configuration, 
            int exceptionsAllowedBeforeBreaking,
            TimeSpan durationOfBreak)
        {
            configuration.SetDefault<IDelegatingHandler, PollyCircuitBreakingDelegatingHandler>(
                new PollyCircuitBreakingDelegatingHandler(
                    exceptionsAllowedBeforeBreaking: exceptionsAllowedBeforeBreaking,
                    durationOfBreak: durationOfBreak
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

        public static ConfigurationBuilder UseLoadBalance(this ConfigurationBuilder configuration)
        {
            UseLoadBalanceFactory(configuration);
            UseRandomLoadBalance(configuration);
            UseRoundRobinLoadBalance(configuration);
            return configuration;
        }

        public static ConfigurationBuilder UseLoadBalanceFactory(this ConfigurationBuilder configuration)
        {
            configuration.SetDefault<ILoadBalanceFactory, LoadBalanceFactory>();
            return configuration;
        }

        public static ConfigurationBuilder UseRandomLoadBalance(this ConfigurationBuilder configuration)
        {
            configuration.SetDefault<ILoadBalanceProvider, RandomLoadBalance>(typeof(RandomLoadBalance).FullName);
            return configuration;
        }

        public static ConfigurationBuilder UseRoundRobinLoadBalance(this ConfigurationBuilder configuration)
        {
            configuration.SetDefault<ILoadBalanceProvider, RoundRobinLoadBalance>(typeof(RoundRobinLoadBalance).FullName);
            return configuration;
        }
    }
}