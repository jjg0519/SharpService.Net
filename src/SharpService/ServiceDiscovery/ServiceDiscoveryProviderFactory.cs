using SharpService.Components;
using SharpService.Configuration;

namespace SharpService.ServiceDiscovery
{
    public class ServiceDiscoveryProviderFactory : IServiceDiscoveryProviderFactory
    {
        private IConfigurationObject configuration { get; set; }

        public ServiceDiscoveryProviderFactory()
        {
            configuration = ObjectContainer.Resolve<IConfigurationObject>();
        }

        public IServiceDiscoveryProvider Get()
        {
            return Get(configuration.registryConfiguration.RegProtocol);
        }

        public IServiceDiscoveryProvider Get(string regProtocol)
        {
            switch (regProtocol)
            {
                case "local":
                    return ObjectContainer.ResolveNamed<IServiceDiscoveryProvider>(typeof(InMemoryDiscoveryProvider).FullName);
                case "consul":
                    return ObjectContainer.ResolveNamed<IServiceDiscoveryProvider>(typeof(ConsulDiscoveryProvider).FullName);
                default:
                    throw new UnableToFindServiceDiscoveryProviderException($"UnableToFindServiceDiscoveryProvider:{regProtocol}");
            }
        }
    }
}
