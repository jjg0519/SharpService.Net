using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;
using SharpService.Utilities;

namespace SharpService.Configuration
{
    public class ClassConfigurationHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            var classConfigurations = new List<ClassConfiguration>();
            var doc = ConfigurationHelper.CreateXmlDoc(section.OuterXml);
            foreach (XmlNode classNode in doc.FirstChild.ChildNodes)
            {
                var errorMessage = string.Empty;
                var classConfiguration = new ClassConfiguration();
                var classProperties = classConfiguration.GetType().GetProperties();
                foreach (var classPropertie in classProperties)
                {
                    var attr = classNode.Attributes[classPropertie.Name.ToLower()];
                    if (attr != null)
                    {
                        classPropertie.SetValue(classConfiguration, Convert.ChangeType(attr.Value, classPropertie.PropertyType), null);
                    }
                }
                if (!ValidateUtil.ValidateEntity(classConfiguration, out errorMessage))
                {
                    throw new Exception(errorMessage);
                }
                classConfigurations.Add(classConfiguration);
            }
            return classConfigurations;
        }
    }
}
