using SharpService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpService.ServiceDiscovery
{
    public class ServiceDiscoveryHelper
    {
        public const string VERSION_PREFIX = "version_";

        public static string GetServiceName(string protocol, string path)
        {
            return $"{protocol}_{path.Replace(".", "-")}";
        }

        public static string GetServiceId(string port, string path)
        {
            var ip = DnsUtil.GetIpAddressAsync().Result;
            return $"{port}_{ip.Replace(".", "-")}_{path.Replace(".", "-")}";
        }

        public static string GetVersionFromStrings(IEnumerable<string> strings)
        {
            return strings
                ?.FirstOrDefault(x => x.StartsWith(VERSION_PREFIX, StringComparison.Ordinal))
                .TrimStart(VERSION_PREFIX.ToCharArray());
        }
    }
}
