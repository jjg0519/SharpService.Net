using SharpService.Utilities;
using System;
using System.Configuration;
using System.Xml;

namespace SharpService.Configuration
{
    class ProtocolConfigurationHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var doc = ConfigurationHelper.CreateXmlDoc(section.OuterXml);
            var protocolConfiguration = new ProtocolConfiguration();
            var registryNode = doc.FirstChild.ChildNodes[0];
            var errorMessage = string.Empty;
            var registryProperties = protocolConfiguration.GetType().GetProperties();
            foreach (var registryPropertie in registryProperties)
            {
                var attr = registryNode.Attributes[registryPropertie.Name.ToLower()];
                if (attr != null)
                {
                    registryPropertie.SetValue(protocolConfiguration, Convert.ChangeType(attr.Value, registryPropertie.PropertyType), null);
                }
            }
            if (!ValidateUtil.ValidateEntity(protocolConfiguration, out errorMessage))
            {
                throw new Exception(errorMessage);
            }
            return protocolConfiguration;
        }
    }
}
