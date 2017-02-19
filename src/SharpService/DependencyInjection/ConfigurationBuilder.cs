using SharpService.Components;

namespace SharpService.DependencyInjection
{
    public class ConfigurationBuilder
    {
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
    }
}
