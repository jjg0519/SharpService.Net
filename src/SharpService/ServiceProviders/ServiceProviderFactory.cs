using SharpService.Configuration;
using SharpService.Components;
using System.Linq;

namespace SharpService.ServiceProviders
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
            return Get(configuration.protocolConfigurations.FirstOrDefault(x => x.Defalut).Protocol);
        }

        public IServiceProvider Get(string protocol)
        {
            switch (protocol)
            {
                case "wcf":
                    return ObjectContainer.ResolveNamed<IServiceProvider>(typeof(WCFServiceProvider).FullName);
                default:
                    throw new UnableToFindServiceProviderException($"UnableToFindServiceProvider:{protocol}");
            }
        }
    }
}
