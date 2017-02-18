using SharpService.Components;
using System;

namespace SharpService.DependencyInjection
{
    public class ConfigurationBuilder
    {
        /// <summary>Provides the singleton access instance.
        /// </summary>
        public static ConfigurationBuilder Instance { get; private set; }

        private ConfigurationBuilder() { }

        public static ConfigurationBuilder Create()
        {
            Instance = new ConfigurationBuilder();
            return Instance;
        }

        public ConfigurationBuilder SetDefault<TService, TImplementer>(string serviceName = null, LifeStyle life = LifeStyle.Singleton)
            where TService : class
            where TImplementer : class, TService
        {
            ObjectContainer.Register<TService, TImplementer>(serviceName, life);
            return this;
        }

        public ConfigurationBuilder SetDefault<TService, TImplementer>(TImplementer instance, string serviceName = null)
            where TService : class
            where TImplementer : class, TService
        {
            ObjectContainer.RegisterInstance<TService, TImplementer>(instance, serviceName);
            return this;
        }

        public ConfigurationBuilder Build()
        {
            ObjectContainer.Build();
            return this;
        }
    }
}
