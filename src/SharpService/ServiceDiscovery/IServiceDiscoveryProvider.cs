using SharpService.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpService.ServiceDiscovery
{
    public interface IServiceDiscoveryProvider
    {
        Task<List<RegistryInformation>> FindAllServicesAsync();

        Task<List<RegistryInformation>> FindServicesAsync(string name);

        Task<List<RegistryInformation>> FindServicesWithVersionAsync(string name, string version);

        Task RegisterServiceAsync();

        Task<RegistryInformation> RegisterServiceAsync(ServiceConfiguration serviceConfiguration, ProtocolConfiguration protocolConfiguration, Uri healthCheckUri = null, IEnumerable<string> tags = null);

        Task DeregisterServiceAsync();

        Task<bool> DeregisterServiceAsync(string serviceId);

    }
}
