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
            var refererElements = new List<RefererConfiguration>();
            var doc = ConfigurationHelper.CreateXmlDoc(section.OuterXml);
            foreach (XmlNode refererNode in doc.FirstChild.ChildNodes)
            {
                var errorMessage = string.Empty;
                var refererElement = new RefererConfiguration();
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
                    var _ref = new Referer() { Interface = refererElement.Interface, Assembly = refererElement.Assembly };
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
                    var uri = HttpUtil.GetUri(_ref.Address);
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
