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
                        OpenTimeout = new TimeSpan(0, 1, 0),
                        CloseTimeout = new TimeSpan(0, 1, 0),
#if DEBUG
                        SendTimeout = new TimeSpan(0, 18, 00),
                        ReceiveTimeout = new TimeSpan(18, 18, 00),
                        MaxBufferSize = int.MaxValue,
                        MaxReceivedMessageSize = int.MaxValue
#else
                        SendTimeout = new TimeSpan(0, 8, 00),
                        ReceiveTimeout = new TimeSpan(0, 18, 00),
                        MaxBufferSize = int.MaxValue,
                        MaxReceivedMessageSize = int.MaxValue
#endif
                    };
                    break;
                case "basichttp":
                    _binding = new BasicHttpBinding(BasicHttpSecurityMode.None)
                    {
                        OpenTimeout = new TimeSpan(0, 1, 0),
                        CloseTimeout = new TimeSpan(0, 1, 0),
#if DEBUG
                        SendTimeout = new TimeSpan(0, 18, 00),
                        ReceiveTimeout = new TimeSpan(18, 18, 00),
                        MaxBufferSize = int.MaxValue,
                        MaxReceivedMessageSize = int.MaxValue
#else
                        SendTimeout = new TimeSpan(0, 8, 00),
                        ReceiveTimeout = new TimeSpan(0, 18, 00),
                        MaxBufferSize = int.MaxValue,
                        MaxReceivedMessageSize = int.MaxValue
#endif
                    };
                    break;
                case "wshttp":
                    _binding = new WSHttpBinding(security)
                    {
                        OpenTimeout = new TimeSpan(0, 1, 0),
                        CloseTimeout = new TimeSpan(0, 1, 0),
#if DEBUG
                        SendTimeout = new TimeSpan(0, 18, 00),
                        ReceiveTimeout = new TimeSpan(18, 18, 00),
                        MaxReceivedMessageSize = int.MaxValue
#else
                    SendTimeout = new TimeSpan(0, 8, 00),
                    ReceiveTimeout = new TimeSpan(0, 18, 00),
                    MaxReceivedMessageSize = int.MaxValue
#endif
                    };
                    break;
                default:
                    _binding = new NetTcpBinding(security)
                    {
                        OpenTimeout = new TimeSpan(0, 1, 0),
                        CloseTimeout = new TimeSpan(0, 1, 0),
#if DEBUG
                        SendTimeout = new TimeSpan(0, 18, 00),
                        ReceiveTimeout = new TimeSpan(18, 18, 00),
                        MaxBufferSize = int.MaxValue,
                        MaxReceivedMessageSize = int.MaxValue
#else
                        SendTimeout = new TimeSpan(0, 8, 00),
                        ReceiveTimeout = new TimeSpan(0, 18, 00),
                        MaxBufferSize = int.MaxValue,
                        MaxReceivedMessageSize = int.MaxValue
#endif
                    };
                    break;
            }
            return _binding;
        }

        public static string CreateAddress(string export, string binding)
        {
            var args = export.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            switch (binding)
            {
                case "nettcp":
                    return $"net.tcp://localhost:{args[1]}/{args[0]}";
                case "basichttp":
                    return $"http://localhost:{args[1]}/{args[0]}";
                case "wshttp":
                    return $"http://localhost:{args[1]}/{args[0]}";
                default:
                    return $"net.tcp://localhost:{args[1]}/{args[0]}";
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
