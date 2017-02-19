using System;
using System.Threading.Tasks;
using SharpService.Utilities;

namespace SharpService.ServiceDiscovery
{
    public class ServiceDiscoveryProvider 
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
    }
}
