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
            List<RefererElement> refererElements = new List<RefererElement>();
            XmlDocument doc = ConfigHelper.CreateXmlDoc(section.InnerXml);
            foreach (XmlNode refererNode in doc.FirstChild.ChildNodes)
            {
                string errorMessage = string.Empty;
                RefererElement refererElement = new RefererElement();
                refererElement.Referers = new List<Referer>();
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
                    Referer _ref = new Referer() { Interface = refererElement.Interface, Assembly = refererElement.Assembly };
                    var refProperties = _ref.GetType().GetProperties();
                    foreach (var refPropertie in refProperties)
                    {
                        var attr = addressNode.Attributes[refPropertie.Name.ToLower()];
                        if (attr != null)
                        {
                            refPropertie.SetValue(_ref, Convert.ChangeType(attr.Value, refPropertie.PropertyType), null);
                        }
                    }

                    if (string.IsNullOrEmpty(_ref.Address))
                    {
                        throw new ArgumentNullException(" the address of the service address cannot be empty");
                    }
                    Uri uri = HttpUtil.GetUri(_ref.Address);
                    _ref.Host = uri.Host;
                    _ref.Port = uri.Port;
                    refererElement.Referers.Add(_ref);               
                }
                if (!ValidateUtil.ValidateEntity(refererElement, out errorMessage))
                {
                    throw new Exception(errorMessage);
                }
                refererElements.Add(refererElement);
            }
            return refererElements;
        }

    }
}
