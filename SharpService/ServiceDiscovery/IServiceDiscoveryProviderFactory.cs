using SharpService.Configuration;

namespace SharpService.ServiceDiscovery
{
    public interface IServiceDiscoveryProviderFactory
    {
        IServiceDiscoveryProvider Get();

        IServiceDiscoveryProvider Get(RegistryConfiguration registryConfig);
    }
}
