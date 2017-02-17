using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using SharpService.Utilities;

namespace SharpService.Configuration
{
   public  class RegistryConfigurationHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var doc = ConfigurationHelper.CreateXmlDoc(section.OuterXml);
            var registryElement = new RegistryElement();
            var registryNode = doc.FirstChild.ChildNodes[0];
            var errorMessage = string.Empty;
            var registryProperties = registryElement.GetType().GetProperties();
            foreach (var registryPropertie in registryProperties)
            {
                var attr = registryNode.Attributes[registryPropertie.Name.ToLower()];
                if (attr != null)
                {
                    registryPropertie.SetValue(registryElement, Convert.ChangeType(attr.Value, registryPropertie.PropertyType), null);
                }
            }
            if (!ValidateUtil.ValidateEntity(registryElement, out errorMessage))
            {
                throw new Exception(errorMessage);
            }
            return registryElement;
        }
    }
}
