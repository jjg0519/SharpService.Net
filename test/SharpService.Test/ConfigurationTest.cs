using SharpService.Configuration;
using NUnit.Framework;
using System.Collections.Generic;
using System.Configuration;

namespace SharpService.Test
{
    public  class ConfigurationTest
    {
        [Test]
        public void TestRefererConfiguration()
        {
            var refererConfig = "serviceGroup/refererConfig";
            var refererConfigurations = ConfigurationManager.GetSection(refererConfig) as List<RefererConfiguration>;
        }

        [Test]
        public void TestServiceConfiguration()
        {
            var serviceConfig = "serviceGroup/serviceConfig";
            var serviceConfigurations = ConfigurationManager.GetSection(serviceConfig) as List<ServiceConfiguration>;
        }

        [Test]
        public void TestClassConfiguration()
        {
            var classConfig = "serviceGroup/classConfig";
            var classConfigurations = ConfigurationManager.GetSection(classConfig) as List<ClassConfiguration>;
        }


        [Test]
        public void TestProtocolConfiguration()
        {
            var protocolConfig = "serviceGroup/protocolConfig";
            var protocolConfiguration = ConfigurationManager.GetSection(protocolConfig) as ProtocolConfiguration;
        }

        [Test]
        public void TestRegistryConfiguration()
        {
            var registryConfig = "serviceGroup/registryConfig";
            var registryConfiguration = ConfigurationManager.GetSection(registryConfig) as RegistryConfiguration;
        }
    }
}
