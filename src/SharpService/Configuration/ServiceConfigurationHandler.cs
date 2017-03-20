using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using SharpService.Utilities;

namespace SharpService.Configuration
{
    public  class ServiceConfigurationHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var serviceConfigurations = new List<ServiceConfiguration>();
            var doc = ConfigurationHelper.CreateXmlDoc(section.OuterXml);
            foreach (XmlNode serviceNode in doc.FirstChild.ChildNodes)
            {
                var errorMessage = string.Empty;
                var serviceConfiguration = new ServiceConfiguration();
                var serviceProperties = serviceConfiguration.GetType().GetProperties();
                foreach (var servicePropertie in serviceProperties)
                {
                    var attr = serviceNode.Attributes[servicePropertie.Name.ToLower()];
                    if (attr != null)
                    {
                        servicePropertie.SetValue(serviceConfiguration, Convert.ChangeType(attr.Value, servicePropertie.PropertyType), null);
                    }
                }
                if (!ValidateUtil.ValidateEntity(serviceConfiguration, out errorMessage))
                {
                    throw new Exception(errorMessage);
                }
                serviceConfigurations.Add(serviceConfiguration);
            }
            return serviceConfigurations;
        }
    }
}
