using SharpService.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace SharpService.Configuration
{
   public class ProtocolConfigurationHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var protocolConfigurations = new List<ProtocolConfiguration>();
            var doc = ConfigurationHelper.CreateXmlDoc(section.OuterXml);
            foreach (XmlNode protocolNode in doc.FirstChild.ChildNodes)
            {
                var errorMessage = string.Empty;
                var protocolConfiguration = new ProtocolConfiguration();
                var protocolProperties = protocolConfiguration.GetType().GetProperties();
                foreach (var protocolPropertie in protocolProperties)
                {
                    var attr = protocolNode.Attributes[protocolPropertie.Name.ToLower()];
                    if (attr != null)
                    {
                        protocolPropertie.SetValue(protocolConfiguration, Convert.ChangeType(attr.Value, protocolPropertie.PropertyType), null);
                    }
                }
                if (!ValidateUtil.ValidateEntity(protocolConfiguration, out errorMessage))
                {
                    throw new Exception(errorMessage);
                }
                protocolConfigurations.Add(protocolConfiguration);
            }
            return protocolConfigurations;
        }
    }
}
