using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using NService.Utilities;

namespace NService.Configuration
{
   public  class ServiceConfigurationHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            List<ServiceElement> serviceElements = new List<ServiceElement>();
            XmlDocument doc = ConfigHelper.CreateXmlDoc(section.InnerXml);
            foreach (XmlNode serviceNode in doc.FirstChild.ChildNodes)
            {
                string errorMessage = string.Empty;
                ServiceElement serviceElement = new ServiceElement();
                var serviceProperties = serviceElement.GetType().GetProperties();
                foreach (var servicePropertie in serviceProperties)
                {
                    var attr = serviceNode.Attributes[servicePropertie.Name.ToLower()];
                    if (attr != null)
                    {
                        servicePropertie.SetValue(serviceElement, Convert.ChangeType(attr.Value, servicePropertie.PropertyType), null);
                    }                 
                }
                if (string.IsNullOrEmpty(serviceElement.Address))
                {
                    throw new ArgumentNullException(" the address of the service address cannot be empty");
                }
                if (serviceElements.Exists(e => e.Address == serviceElement.Address))
                {
                    throw new ArgumentNullException(" the address of the service address cannot be same");
                }
                if (!ValidateUtil.ValidateEntity(serviceElement, out errorMessage))
                {
                    throw new Exception(errorMessage);
                }
                serviceElements.Add(serviceElement);
            }
            return serviceElements;       
        }
    }
}
