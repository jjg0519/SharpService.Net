using SharpService.Configuration;
using System.Configuration;

namespace SharpService.ServiceDiscovery
{
    public class ServiceDiscoveryProviderFactory : IServiceDiscoveryProviderFactory
    {
        private const string registryConfig = "serviceGroup/registryConfig";
        private readonly RegistryConfiguration registryConfiguration = ConfigurationManager.GetSection(registryConfig) as RegistryConfiguration;

        public IServiceDiscoveryProvider Get()
        {
            return Get(registryConfiguration);
        }

        public IServiceDiscoveryProvider Get(RegistryConfiguration registryConfig)
        {
            switch (registryConfig.RegProtocol)
            {
                case "":
                    return new InMemoryDiscoveryProvider();
                default:
                    throw new UnableToFindServiceDiscoveryProviderException("UnableToFindServiceDiscoveryProvider");
            }
        }
    }
}
