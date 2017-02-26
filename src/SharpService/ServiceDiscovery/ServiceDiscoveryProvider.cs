using System;
using System.Threading.Tasks;
using SharpService.Utilities;
using SharpService.Configuration;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace SharpService.ServiceDiscovery
{
    public class ServiceDiscoveryProvider
    {
        protected const string serviceConfig = "serviceGroup/serviceConfig";
        protected readonly List<ServiceConfiguration> serviceConfigurations = ConfigurationManager.GetSection(serviceConfig) as List<ServiceConfiguration>;

        protected readonly List<RegistryInformation> ServiceInstances = new List<RegistryInformation>();

        protected const string SERVERPATH_PREFIX = "sharpservice";
        protected const string SERVERNAME_PREFIX = "server_";
        protected const string VERSION_PREFIX = "version_";

        public virtual Task<string> GetServiceNameAsync(string @interface, string assembly)
        {
            return Task.FromResult($"{SERVERNAME_PREFIX}{@interface}_{assembly}");
        }

        public virtual async Task<string> GetServiceIdAsync(string serviceName, Uri uri)
        {
            var ipAddress = await DnsUtil.GetIpAddressAsync();
            return $"{serviceName}_{ipAddress.Replace(".", "_")}_{uri.Port}";
        }

        public virtual Task<string> GetVersionAsync(string version)
        {
            return Task.FromResult($"{VERSION_PREFIX}{version}");
        }

        public virtual string GetVersionFromStrings(IEnumerable<string> strings)
        {
            return strings
                ?.FirstOrDefault(x => x.StartsWith(VERSION_PREFIX, StringComparison.Ordinal))
                .TrimStart(VERSION_PREFIX.ToCharArray());
        }
    }
}
