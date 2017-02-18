using SharpService.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpService.ServiceDiscovery
{
    public class InMemoryDiscoveryProvider : IServiceDiscoveryProvider
    {
        private const string serviceConfig = "serviceGroup/serviceConfig";
        private const string classConfig = "serviceGroup/classConfig";
        private readonly List<ServiceConfiguration> serviceConfigurations = ConfigurationManager.GetSection(serviceConfig) as List<ServiceConfiguration>;
        private readonly List<ClassConfiguration> classConfigurations = ConfigurationManager.GetSection(classConfig) as List<ClassConfiguration>;

        private static ConcurrentDictionary<string, ConcurrentDictionary<string, string>> memoryDic = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();


    }
}
