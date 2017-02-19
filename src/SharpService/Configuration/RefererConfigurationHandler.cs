using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using System.Linq;
using SharpService.Utilities;

namespace SharpService.Configuration
{
    public class RefererConfigurationHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var refererConfigurations = new List<RefererConfiguration>();
            var doc = ConfigurationHelper.CreateXmlDoc(section.OuterXml);
            foreach (XmlNode refererNode in doc.FirstChild.ChildNodes)
            {
                var errorMessage = string.Empty;
                var refererConfiguration = new RefererConfiguration();
                var refererProperties = refererConfiguration.GetType().GetProperties();
                foreach (var refererPropertie in refererProperties)
                {
                    var attr = refererNode.Attributes[refererPropertie.Name.ToLower()];
                    if (attr != null)
                    {
                        refererPropertie.SetValue(refererConfiguration, Convert.ChangeType(attr.Value, refererPropertie.PropertyType), null);
                    }
                }
                if (!ValidateUtil.ValidateEntity(refererConfiguration, out errorMessage))
                {
                    throw new Exception(errorMessage);
                }
                refererConfigurations.Add(refererConfiguration);
            }
            return refererConfigurations;
        }

    }
}
