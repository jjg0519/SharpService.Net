using SharpService.Configuration;
using NUnit.Framework;
using System.Collections.Generic;
using System.Configuration;

namespace SharpService.Test
{
    public  class ConfigurationTest
    {
        [Test]
        public void TestRefererConfig()
        {
            var refererConfig = "serviceGroup/refererConfig";
            var refererConfigurations = ConfigurationManager.GetSection(refererConfig) as List<RefererConfiguration>;
        }

        [Test]
        public void TestServiceConfig()
        {
            var serviceConfig = "serviceGroup/serviceConfig";
            var serviceConfigurations = ConfigurationManager.GetSection(serviceConfig) as List<ServiceConfiguration>;
        }

        [Test]
        public void TestClassConfig()
        {
            var classConfig = "serviceGroup/classConfig";
            var classConfigurations = ConfigurationManager.GetSection(classConfig) as List<ClassConfiguration>;
        }

        [Test]
        public void TestRegistryConfig()
        {
            var registryConfig = "serviceGroup/registryConfig";
            var registryConfiguration = ConfigurationManager.GetSection(registryConfig) as RegistryConfiguration;
        }
    }
}
