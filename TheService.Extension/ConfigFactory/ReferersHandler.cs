using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using System.Linq;
using TheService.Common;

namespace TheService.Extension.ConfigFactory
{
    public  class ReferersHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            List<RefererElement> listReferers = new List<RefererElement>();
            XmlDocument doc = ConfigHelper.CreateXmlDoc(section.InnerXml);
            foreach (XmlNode refererNode in doc.FirstChild.ChildNodes)
            {
                string errorMessage = string.Empty;
                RefererElement refererElement = new RefererElement();
                refererElement.Addresss = new List<string>();
                var refererProperties = refererElement.GetType().GetProperties();
                foreach (var refererPropertie in refererProperties)
                {
                    var attr = refererNode.Attributes[refererPropertie.Name.ToLower()];
                    if (attr != null)
                    {
                        refererPropertie.SetValue(refererElement, Convert.ChangeType(attr.Value, refererPropertie.PropertyType), null);
                    }                 
                }                       
                foreach (XmlNode addressNode in refererNode.ChildNodes)
                {
                    var address = addressNode.Attributes["address"].Value;
                    if (!string.IsNullOrEmpty(address))
                    {
                        Utility.UrlCheck(address);
                        refererElement.Addresss.Add(address);
                    }
                    else
                    {
                        throw new Exception(" the address of the service address cannot be empty");
                    }
                }
                if (refererElement.Addresss.Count() == 0)
                {
                    throw new Exception(" the address of the service address cannot be empty");
                }
                if (refererElement.Addresss.Distinct().Count() != refererElement.Addresss.Count())
                {
                    throw new Exception(" the address of the service address cannot be same");
                }
                if (!ValidateHelper.ValidateEntity(refererElement, out errorMessage))
                {
                    throw new Exception(errorMessage);
                }
                listReferers.Add(refererElement);
            }
            return listReferers;
        }
    }
}
