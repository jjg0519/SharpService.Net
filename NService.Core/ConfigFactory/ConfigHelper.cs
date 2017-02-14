using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace NService.Core.ConfigFactory
{
    public class ConfigHelper
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
                case "netnamedpipe":
                    _binding = new NetNamedPipeBinding()
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
                case "netmsmq":
                    _binding = new NetMsmqBinding()
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

        public static XmlDocument CreateXmlDoc(string innerXml)
        {
            XmlDocument doc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            using (StringReader strRdr = new StringReader(innerXml))
            {
                using (XmlReader reader = XmlReader.Create(strRdr, settings))
                {
                    doc.Load(reader);
                }
            }
            return doc;
        }
    }
}
