using NService.Configuration;
using NUnit.Framework;
using System.Collections.Generic;
using System.Configuration;

namespace NService.Test
{
    public  class ConfigurationTest
    {
        [Test]
        public void TestRefererConfig()
        {
            string refererConfig = "serviceGroup/refererConfig";
            List<RefererElement> refererElements = ConfigurationManager.GetSection(refererConfig) as List<RefererElement>;
        }

        [Test]
        public void TestServiceConfig()
        {
            string serviceConfig = "serviceGroup/serviceConfig";
            List<ServiceElement> services = ConfigurationManager.GetSection(serviceConfig) as List<ServiceElement>;
        }

        [Test]
        public void TestClassConfig()
        {
            string classConfig = "serviceGroup/classConfig";
            List<ClassElement> classs = ConfigurationManager.GetSection(classConfig) as List<ClassElement>;
        }
    }
}
