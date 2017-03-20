using SharpService.Configuration;
using SharpService.Components;

namespace SharpService.ServiceProvider
{
    public class ServiceProviderFactory : IServiceProviderFactory
    {
        private IConfigurationObject configuration { get; set; }

        public ServiceProviderFactory()
        {
            configuration = ObjectContainer.Resolve<IConfigurationObject>();
        }

        public IServiceProvider Get()
        {
            return Get(configuration.protocolConfiguration);
        }

        public IServiceProvider Get(ProtocolConfiguration protocolConfig)
        {
            switch (protocolConfig.Protocol)
            {
                case "wcf":
                    return ObjectContainer.ResolveNamed<IServiceProvider>(typeof(WCFServiceProvider).FullName);
                default:
                    throw new UnableToFindServiceProviderException($"UnableToFindServiceDiscoveryProvider:{protocolConfig.Protocol}");
            }
        }
    }
}
