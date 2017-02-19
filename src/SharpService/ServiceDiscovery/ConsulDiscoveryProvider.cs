using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpService.Configuration;

namespace SharpService.ServiceDiscovery
{
    public class ConsulDiscoveryProvider : ServiceDiscoveryProvider, IServiceDiscoveryProvider
    {
        public Task<bool> DeregisterServiceAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeregisterServiceAsync(string serviceId)
        {
            throw new NotImplementedException();
        }

        public Task<List<RegistryInformation>> FindAllServicesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<RegistryInformation>> FindServicesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<RegistryInformation>> FindServicesAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<RegistryInformation>> FindServicesWithVersionAsync(string name, string version)
        {
            throw new NotImplementedException();
        }

        public Task RegisterServiceAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RegistryInformation> RegisterServiceAsync(ServiceConfiguration serviceConfig)
        {
            throw new NotImplementedException();
        }

        public Task<RegistryInformation> RegisterServiceAsync(string serviceName, string version, Uri uri, List<string> tags = null)
        {
            throw new NotImplementedException();
        }
    }
}
