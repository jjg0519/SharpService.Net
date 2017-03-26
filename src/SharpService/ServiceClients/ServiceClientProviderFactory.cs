using SharpService.Configuration;
using SharpService.Components;
using System.Linq;

namespace SharpService.ServiceClients
{
    public class ServiceClientProviderFactory : IServiceClientProviderFactory
    {
        private IConfigurationObject configuration { get; set; }

        public ServiceClientProviderFactory()
        {
            configuration = ObjectContainer.Resolve<IConfigurationObject>();
        }

        public IServiceClientProvider Get()
        {
            return Get(configuration.protocolConfigurations.FirstOrDefault(x => x.Defalut).Protocol);
        }

        public IServiceClientProvider Get(string protocol)
        {
            switch (protocol)
            {
                case "wcf":
                    return ObjectContainer.ResolveNamed<IServiceClientProvider>(typeof(WCFServiceClientProvider).FullName);
                default:
                    throw new UnableToFindServiceClientProviderException($"UnableToFindServiceClientProvider:{protocol}");
            }
        }
    }
}
