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
            var classElements = new List<ClassElement>();
            var doc = ConfigurationHelper.CreateXmlDoc(section.OuterXml);
            foreach (XmlNode classNode in doc.FirstChild.ChildNodes)
            {
                var errorMessage = string.Empty;
                var classElement = new ClassElement();
                var classProperties = classElement.GetType().GetProperties();
                foreach (var classPropertie in classProperties)
                {
                    var attr = classNode.Attributes[classPropertie.Name.ToLower()];
                    if (attr != null)
                    {
                        classPropertie.SetValue(classElement, Convert.ChangeType(attr.Value, classPropertie.PropertyType), null);
                    }
                }
                if (!ValidateUtil.ValidateEntity(classElement, out errorMessage))
                {
                    throw new Exception(errorMessage);
                }
                classElements.Add(classElement);
            }
            return classElements;
        }
    }
}
