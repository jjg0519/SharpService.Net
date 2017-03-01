using SharpService.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace SharpService.Configuration
{
    public class ConfigurationHelper
    {
        public static Binding CreateBinding(string binding, SecurityMode security)
        {
            Binding _binding = null;
            switch (binding)
            {
                case "nettcp":
                    _binding = new NetTcpBinding(security)
                    {
                        OpenTimeout = new TimeSpan(0, 10, 0),
                        CloseTimeout = new TimeSpan(0, 10, 0),
                        SendTimeout = new TimeSpan(0, 10, 0),
                        ReceiveTimeout = new TimeSpan(0, 10, 0),
                        MaxBufferSize = 2147483647,
                        MaxReceivedMessageSize = 2147483647,
                        ReaderQuotas = new XmlDictionaryReaderQuotas()
                        {
                            MaxDepth = 2147483647,
                            MaxStringContentLength = 2147483647,
                            MaxArrayLength = 2147483647,
                            MaxBytesPerRead = 2147483647,
                            MaxNameTableCharCount = 2147483647
                        }
                    };
                    break;
                case "basichttp":
                    _binding = new BasicHttpBinding(BasicHttpSecurityMode.None)
                    {
                        OpenTimeout = new TimeSpan(0, 10, 0),
                        CloseTimeout = new TimeSpan(0, 10, 0),
                        SendTimeout = new TimeSpan(0, 10, 0),
                        ReceiveTimeout = new TimeSpan(0, 10, 0),
                        MaxBufferSize = 2147483647,
                        MaxReceivedMessageSize = 2147483647,
                        ReaderQuotas = new XmlDictionaryReaderQuotas()
                        {
                            MaxDepth = 2147483647,
                            MaxStringContentLength = 2147483647,
                            MaxArrayLength = 2147483647,
                            MaxBytesPerRead = 2147483647,
                            MaxNameTableCharCount = 2147483647
                        }
                    };
                    break;
                case "wshttp":
                    _binding = new WSHttpBinding(security)
                    {
                        OpenTimeout = new TimeSpan(0, 10, 0),
                        CloseTimeout = new TimeSpan(0, 10, 0),
                        SendTimeout = new TimeSpan(0, 10, 0),
                        ReceiveTimeout = new TimeSpan(0, 10, 0),
                        MaxReceivedMessageSize = 2147483647,
                        ReaderQuotas = new XmlDictionaryReaderQuotas()
                        {
                            MaxDepth = 2147483647,
                            MaxStringContentLength = 2147483647,
                            MaxArrayLength = 2147483647,
                            MaxBytesPerRead = 2147483647,
                            MaxNameTableCharCount = 2147483647
                        }
                    };
                    break;
                default:
                    _binding = new NetTcpBinding(security)
                    {
                        OpenTimeout = new TimeSpan(0, 10, 0),
                        CloseTimeout = new TimeSpan(0, 10, 0),
                        SendTimeout = new TimeSpan(0, 10, 0),
                        ReceiveTimeout = new TimeSpan(0, 10, 0),
                        MaxBufferSize = 2147483647,
                        MaxReceivedMessageSize = 2147483647,
                        ReaderQuotas = new XmlDictionaryReaderQuotas()
                        {
                            MaxDepth = 2147483647,
                            MaxStringContentLength = 2147483647,
                            MaxArrayLength = 2147483647,
                            MaxBytesPerRead = 2147483647,
                            MaxNameTableCharCount = 2147483647
                        }
                    };
                    break;
            }
            return _binding;
        }

        public static string CreateAddress(string export, string binding)
        {
            var args = export.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            var ipAddress = DnsUtil.GetIpAddressAsync().Result;
            switch (binding)
            {
                case "nettcp":
                    return $"net.tcp://{ipAddress}:{args[1]}/{args[0]}";
                case "basichttp":
                    return $"http://{ipAddress}:{args[1]}/{args[0]}";
                case "wshttp":
                    return $"http://{ipAddress}:{args[1]}/{args[0]}";
                default:
                    return $"net.tcp://{ipAddress}:{args[1]}/{args[0]}";
            }
        }

        public static XmlDocument CreateXmlDoc(string innerXml)
        {
            var doc = new XmlDocument();
            var settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            using (var strRdr = new StringReader(innerXml))
            {
                using (var reader = XmlReader.Create(strRdr, settings))
                {
                    doc.Load(reader);
                }
            }
            return doc;
        }
    }
}
