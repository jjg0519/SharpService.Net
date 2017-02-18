using SharpService.Configuration;
using System.Threading.Tasks;

namespace SharpService.ServiceDiscovery
{
    public interface IServiceDiscoveryProviderFactory
    {
        Task<IServiceDiscoveryProvider> GetAsync();

        Task<IServiceDiscoveryProvider> GetAsync(RegistryConfiguration registryConfig);
    }
}
