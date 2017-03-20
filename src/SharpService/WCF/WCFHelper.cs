using SharpService.Utilities;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

namespace SharpService.WCF
{
    public class WCFHelper
    {
        public static Binding CreateBinding(string binding, SecurityMode security = SecurityMode.None)
        {
            Binding _binding = null;
            switch (binding)
            {
                case "tcp":
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
                case "http":
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

        public static string CreateAddress(string transmit, string port, string name)
        {
            var ipAddress = DnsUtil.GetIpAddressAsync().Result;
            switch (transmit)
            {
                case "tcp":
                    return $"net.tcp://{ipAddress}:{port}/{name}";
                case "http":
                    return $"http://{ipAddress}:{port}/{name}";
                default:
                    return $"net.tcp://{ipAddress}:{port}/{name}";
            }
        }
    }
}
