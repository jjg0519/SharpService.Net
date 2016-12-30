using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;
using TheService.Common;

namespace TheService.Extension.ConfigFactory
{
    public class ReferencesHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            List<ReferenceElement> listReferences = new List<ReferenceElement>();
            XmlDocument doc = ConfigHelper.CreateXmlDoc(section.InnerXml);
            foreach (XmlNode referenceNode in doc.FirstChild.ChildNodes)
            {
                string errorMessage = string.Empty;
                ReferenceElement referenceElement = new ReferenceElement();
                var referenceProperties = referenceElement.GetType().GetProperties();
                foreach (var servicePropertie in referenceProperties)
                {
                    var attr = referenceNode.Attributes[servicePropertie.Name.ToLower()];
                    if (attr != null)
                    {
                        servicePropertie.SetValue(referenceElement, Convert.ChangeType(attr.Value, servicePropertie.PropertyType), null);
                    }
                }
                if (!ValidateHelper.ValidateEntity(referenceElement, out errorMessage))
                {
                    throw new Exception(errorMessage);
                }
                listReferences.Add(referenceElement);
            }
            return listReferences;
        }
    }
}
