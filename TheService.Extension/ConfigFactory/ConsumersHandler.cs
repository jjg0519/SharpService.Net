using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using System.Linq;
using TheService.Common;

namespace TheService.Extension.ConfigFactory
{
    public  class ConsumersHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            List<ConsumerElement> listConsumers = new List<ConsumerElement>();
            XmlDocument doc = ConfigHelper.CreateXmlDoc(section.InnerXml);
            foreach (XmlNode consumerNode in doc.FirstChild.ChildNodes)
            {
                string errorMessage = string.Empty;
                ConsumerElement consumerElement = new ConsumerElement();
                var consumerProperties = consumerElement.GetType().GetProperties();
                foreach (var consumerPropertie in consumerProperties)
                {
                    var attr = consumerNode.Attributes[consumerPropertie.Name.ToLower()];
                    if (attr != null)
                    {
                        consumerPropertie.SetValue(consumerElement, Convert.ChangeType(attr.Value, consumerPropertie.PropertyType), null);
                    }                 
                }
                consumerElement.Addresss = new List<string>();
                foreach (XmlNode addressNode in consumerNode.ChildNodes)
                {
                    var address = addressNode.Attributes["address"].Value;
                    if (!string.IsNullOrEmpty(address))
                    {
                        Utility.UrlCheck(address);
                        consumerElement.Addresss.Add(address);
                    }
                    else
                    {
                        throw new Exception(" the address of the service address cannot be empty");
                    }
                }
                if (consumerElement.Addresss.Count() == 0)
                {
                    throw new Exception(" the address of the service address cannot be empty");
                }
                if (consumerElement.Addresss.Distinct().Count() != consumerElement.Addresss.Count())
                {
                    throw new Exception(" the address of the service address cannot be same");
                }
                if (!ValidateHelper.ValidateEntity(consumerElement, out errorMessage))
                {
                    throw new Exception(errorMessage);
                }
                listConsumers.Add(consumerElement);
            }
            return listConsumers;
        }
    }
}
