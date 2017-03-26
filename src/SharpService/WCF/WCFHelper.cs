using ProtoBuf.ServiceModel;
using SharpService.ServiceDiscovery;
using SharpService.Utilities;
using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

namespace SharpService.WCF
{
    public class WCFHelper
    {
        public static ChannelFactory<IService> CreateChannelFactory<IService>(RegistryInformation registryInformation)
        {
            var transmit = registryInformation.Tags.ToList()[1];
            var binding = CreateBinding(transmit);
            var address = CreateAddress(
                transmit,
                registryInformation.Address,
                registryInformation.Port.ToString(),
                typeof(IService).Name.TrimStart('I').ToLower());
            var endpoint = new EndpointAddress(address);
            var factory = new ChannelFactory<IService>(binding, endpoint);
            factory.Endpoint.Behaviors.Add(new ProtoEndpointBehavior());
            return factory;
        }

        public static Binding CreateBinding(string transmit, SecurityMode security = SecurityMode.None)
        {
            Binding _binding = null;
            switch (transmit)
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
                    _binding = new BasicHttpBinding()
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
                    throw new ArgumentException($"can not find transmit:{transmit}");
            }
            return _binding;
        }

        public static string CreateAddress(string transmit, string port, string name)
        {
            var ip = DnsUtil.GetIpAddressAsync().Result;
            return CreateAddress(transmit, ip, port, name);
        }

        public static string CreateAddress(string transmit, string ip, string port, string name)
        {
            switch (transmit)
            {
                case "tcp":
                    return $"net.tcp://{ip}:{port}/{name}";
                case "http":
                    return $"http://{ip}:{port}/{name}";
                default:
                    throw new ArgumentException($"can not find transmit:{transmit}");
            }
        }
    }
}
