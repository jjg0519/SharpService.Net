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
            var refererElements = ConfigurationManager.GetSection(refererConfig) as List<RefererElement>;
        }

        [Test]
        public void TestServiceConfig()
        {
            var serviceConfig = "serviceGroup/serviceConfig";
            var services = ConfigurationManager.GetSection(serviceConfig) as List<ServiceElement>;
        }

        [Test]
        public void TestClassConfig()
        {
            var classConfig = "serviceGroup/classConfig";
            var classs = ConfigurationManager.GetSection(classConfig) as List<ClassElement>;
        }

        [Test]
        public void TestRegistryConfig()
        {
            var registryConfig = "serviceGroup/registryConfig";
            var registry = ConfigurationManager.GetSection(registryConfig) as RegistryElement;
        }
    }
}
