using SharpService.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpService.ServiceDiscovery
{
    public interface IServiceDiscoveryProvider
    {
        Task<string> GetServiceNameAsync(string @interface, string @assembly);

        Task<string> GetServiceIdAsync(string serviceName, Uri uri);

        Task RegisterServiceAsync();

        Task<RegistryInformation> RegisterServiceAsync(ServiceConfiguration serviceConfig);

        Task<RegistryInformation> RegisterServiceAsync(string serviceName, string version, Uri uri, List<string> tags = null);

        Task<bool> DeregisterServiceAsync(string serviceId);

        Task<bool> DeregisterServiceAsync();

        Task<List<RegistryInformation>> FindServicesAsync();

        Task<List<RegistryInformation>> FindServicesAsync(string name);

        Task<List<RegistryInformation>> FindServicesWithVersionAsync(string name, string version);

    }
}
