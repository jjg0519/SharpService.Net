using SharpService.Configuration;
using SharpService.ServiceDiscovery;

namespace SharpService.ServiceClients
{
    public interface IServiceClientProvider
    {
        IService GetServiceClient<IService>(RegistryInformation registryInformation, ProtocolConfiguration protocolConfiguration);
    }
}
