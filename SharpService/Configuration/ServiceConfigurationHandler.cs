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
   public  class ServiceConfigurationHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var serviceElements = new List<ServiceElement>();
            var doc = ConfigurationHelper.CreateXmlDoc(section.OuterXml);
            foreach (XmlNode serviceNode in doc.FirstChild.ChildNodes)
            {
                var errorMessage = string.Empty;
                var serviceElement = new ServiceElement();
                var serviceProperties = serviceElement.GetType().GetProperties();
                foreach (var servicePropertie in serviceProperties)
                {
                    var attr = serviceNode.Attributes[servicePropertie.Name.ToLower()];
                    if (attr != null)
                    {
                        servicePropertie.SetValue(serviceElement, Convert.ChangeType(attr.Value, servicePropertie.PropertyType), null);
                    }
                }
                serviceElement.Address = ConfigurationHelper.CreateAddress(serviceElement.Export, serviceElement.Binding);
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
