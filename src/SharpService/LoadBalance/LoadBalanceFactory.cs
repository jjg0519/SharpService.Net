using SharpService.Components;
using SharpService.Configuration;

namespace SharpService.LoadBalance
{
    public class LoadBalanceFactory : ILoadBalanceFactory
    {
        private IConfigurationObject configuration { get; set; }

        public LoadBalanceFactory()
        {
            configuration = ObjectContainer.Resolve<IConfigurationObject>();
        }

        public ILoadBalanceProvider Get()
        {
            return Get(configuration.protocolConfiguration);
        }

        public ILoadBalanceProvider Get(ProtocolConfiguration protocolConfiguration)
        {
            switch (protocolConfig.LoadBalance)
            {
                case "random":
                    return ObjectContainer.ResolveNamed<ILoadBalanceProvider>(typeof(RandomLoadBalance).FullName);
                case "roundrobin":
                    return ObjectContainer.ResolveNamed<ILoadBalanceProvider>(typeof(RoundRobinLoadBalance).FullName);
                default:
                    throw new UnableToFindLoadBalanceException($"UnableToFindServiceDiscoveryProvider:{protocolConfig.LoadBalance}");
            }
        }
    }
}
