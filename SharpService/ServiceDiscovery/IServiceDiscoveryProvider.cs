using SharpService.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpService.ServiceDiscovery
{
    public interface IServiceDiscoveryProvider
    {
        Task<string> GetServiceName(string @interface,string @assembly);

        Task<string> GetServiceIdAsync(string serviceName, Uri uri);

        Task<RegistryInformation> RegisterServiceAsync(ServiceConfiguration serviceConfig);

        Task<RegistryInformation> RegisterServiceAsync(string serviceName, string version, Uri uri,  IEnumerable<string> tags = null);

        Task<bool> DeregisterServiceAsync(string serviceId);

        Task<IList<RegistryInformation>> FindServicesAsync();

        Task<IList<RegistryInformation>> FindServicesAsync(string name);

        Task<IList<RegistryInformation>> FindServicesWithVersionAsync(string name, string version);

        Task<IList<RegistryInformation>> FindAllServicesAsync();
    }
}
