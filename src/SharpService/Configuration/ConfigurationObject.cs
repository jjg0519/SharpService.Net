using System.Collections.Generic;
using System.Configuration;

namespace SharpService.Configuration
{
    public  class ConfigurationObject : IConfigurationObject
    {
        public List<RefererConfiguration> refererConfigurations
        {
            get
            {
                var refererConfig = "serviceGroup/refererConfig";
                return ConfigurationManager.GetSection(refererConfig) as List<RefererConfiguration>;
            }
        }

        public List<ServiceConfiguration> serviceConfigurations
        {
            get
            {
                var serviceConfig = "serviceGroup/serviceConfig";
                return ConfigurationManager.GetSection(serviceConfig) as List<ServiceConfiguration>;
            }
        }

        public List<ClassConfiguration> classConfigurations
        {
            get
            {
                var classConfig = "serviceGroup/classConfig";
                return ConfigurationManager.GetSection(classConfig) as List<ClassConfiguration>;
            }
        }

        public ProtocolConfiguration protocolConfiguration
        {
            get
            {
                var protocolConfig = "serviceGroup/protocolConfig";
                return ConfigurationManager.GetSection(protocolConfig) as ProtocolConfiguration;
            }
        }
       
        public RegistryConfiguration registryConfiguration
        {
            get
            {
                var registryConfig = "serviceGroup/registryConfig";
                return ConfigurationManager.GetSection(registryConfig) as RegistryConfiguration;
            }
        }         
    }
}
