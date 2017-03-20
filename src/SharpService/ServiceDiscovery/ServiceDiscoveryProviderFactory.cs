using SharpService.Components;
using SharpService.Configuration;

namespace SharpService.ServiceDiscovery
{
    public class ServiceDiscoveryProviderFactory : IServiceDiscoveryProviderFactory
    {
        private IConfigurationObject configuration { get; set; }

        public ServiceDiscoveryProviderFactory()
        {
            configuration = ObjectContainer.Resolve<IConfigurationObject>();
        }

        public IServiceDiscoveryProvider Get()
        {
            return Get(configuration.registryConfiguration);
        }

        public IServiceDiscoveryProvider Get(RegistryConfiguration registryConfig)
        {
            switch (registryConfig.RegProtocol)
            {
                case "consul":
                    return ObjectContainer.ResolveNamed<IServiceDiscoveryProvider>(typeof(ConsulDiscoveryProvider).FullName);
                default:
                    throw new UnableToFindServiceDiscoveryProviderException($"UnableToFindServiceDiscoveryProvider:{registryConfig.RegProtocol}");
            }
        }
    }
}
