﻿using Autofac;
using Polly.Timeout;
using SharpService.Components;
using SharpService.LoadBalance;
using SharpService.Logging;
using SharpService.ServiceRequester;
using SharpService.ServiceDiscovery;
using SharpService.ServiceProvider;
using System;
using SharpService.Configuration;

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

        public static ConfigurationBuilder UseConfigurationObject(this ConfigurationBuilder configuration)
        {
            configuration.SetDefault<IConfigurationObject, ConfigurationObject>();
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

        public static ConfigurationBuilder UseServiceProviderFactory(this ConfigurationBuilder configuration)
        {
            configuration.SetDefault<IServiceProviderFactory, ServiceProviderFactory>();
            return configuration;
        }

        public static ConfigurationBuilder UseServiceProvider(this ConfigurationBuilder configuration)
        {
            UseWCFServiceProvider(configuration);
            return configuration;
        }

        public static ConfigurationBuilder UseWCFServiceProvider(this ConfigurationBuilder configuration)
        {
            configuration.SetDefault<ServiceProvider.IServiceProvider, WCFServiceProvider>(typeof(WCFServiceProvider).FullName);
            return configuration;
        }

        public static ConfigurationBuilder UseServiceDiscoveryProvider(this ConfigurationBuilder configuration)
        {
            UseServiceDiscoveryProviderFactory(configuration);
            UseConsulDiscoveryProvider(configuration);
            return configuration;
        }

        public static ConfigurationBuilder UseServiceDiscoveryProviderFactory(this ConfigurationBuilder configuration)
        {
            configuration.SetDefault<IServiceDiscoveryProviderFactory, ServiceDiscoveryProviderFactory>();
            return configuration;
        }

        public static ConfigurationBuilder UseConsulDiscoveryProvider(this ConfigurationBuilder configuration)
        {
            configuration.SetDefault<IServiceDiscoveryProvider, ConsulDiscoveryProvider>(typeof(ConsulDiscoveryProvider).FullName);
            return configuration;
        }

        public static ConfigurationBuilder UseLog4Net(this ConfigurationBuilder configuration)
        {
            return UseLog4Net(configuration, "log4net.config");
        }

        public static ConfigurationBuilder UseLog4Net(this ConfigurationBuilder configuration, string configFile)
        {
            configuration.SetDefault<ILogger, Log4NetLogger>(new Log4NetLogger(configFile));
            return configuration;
        }

        public static ConfigurationBuilder UseExceptionlessLogger(this ConfigurationBuilder configuration)
        {
            configuration.SetDefault<ILogger, ExceptionlessLogger>();
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