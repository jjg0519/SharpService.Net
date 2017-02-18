using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpService.Configuration;
using SharpService.Utilities;

namespace SharpService.ServiceDiscovery
{
    public class ServiceDiscoveryProvider : IServiceDiscoveryProvider
    {
        protected const string SERVERNAME_PREFIX = "server_";
        protected const string VERSION_PREFIX = "version_";

        public virtual Task<string> GetServiceName(string @interface, string assembly)
        {
            return Task.FromResult($"{SERVERNAME_PREFIX}{@interface}_{assembly}");
        }

        public virtual async Task<string> GetServiceIdAsync(string serviceName, Uri uri)
        {
            var ipAddress = await DnsUtil.GetIpAddressAsync();
            return $"{serviceName}_{ipAddress.Replace(".", "_")}_{uri.Port}";
        }
   
        public virtual Task<bool> DeregisterServiceAsync(string serviceId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> DeregisterServiceAsync()
        {
            throw new NotImplementedException();
        }

        public virtual Task<RegistryInformation> RegisterServiceAsync(ServiceConfiguration serviceConfig)
        {
            throw new NotImplementedException();
        }

        public virtual Task<RegistryInformation> RegisterServiceAsync(string serviceName, string version, Uri uri, List<string> tags = null)
        {
            throw new NotImplementedException();
        }

        public virtual Task RegisterServiceAsync()
        {
            throw new NotImplementedException();
        }

        public virtual Task<List<RegistryInformation>> FindServicesAsync()
        {
            throw new NotImplementedException();
        }

        public virtual Task<List<RegistryInformation>> FindServicesAsync(string name)
        {
            throw new NotImplementedException();
        }

        public virtual Task<List<RegistryInformation>> FindServicesWithVersionAsync(string name, string version)
        {
            throw new NotImplementedException();
        }

        public virtual Task<List<RegistryInformation>> FindAllServicesAsync()
        {
            throw new NotImplementedException();
        }    
    }
}
