using SharpService.Components;
using SharpService.Configuration;
using System.Configuration;
using System.Threading.Tasks;

namespace SharpService.ServiceDiscovery
{
    public class ServiceDiscoveryProviderFactory : IServiceDiscoveryProviderFactory
    {
        private const string registryConfig = "serviceGroup/registryConfig";
        private readonly RegistryConfiguration registryConfiguration = ConfigurationManager.GetSection(registryConfig) as RegistryConfiguration;

        public Task<IServiceDiscoveryProvider> GetAsync()
        {
            return GetAsync(registryConfiguration);
        }

        public Task<IServiceDiscoveryProvider> GetAsync(RegistryConfiguration registryConfig)
        {
            switch (registryConfig.RegProtocol)
            {
                case "local":
                    return Task.FromResult(ObjectContainer.ResolveNamed<IServiceDiscoveryProvider>(
                        typeof(InMemoryDiscoveryProvider).FullName));
                case "zookeeper":
                    return Task.FromResult(ObjectContainer.ResolveNamed<IServiceDiscoveryProvider>(
                        typeof(ZooKeeperDiscoveryProvider).FullName));
                case "consul":
                    return Task.FromResult(ObjectContainer.ResolveNamed<IServiceDiscoveryProvider>(
                        typeof(ConsulDiscoveryProvider).FullName));
                default:
                    throw new UnableToFindServiceDiscoveryProviderException("UnableToFindServiceDiscoveryProvider");
            }
        }
    }
}
