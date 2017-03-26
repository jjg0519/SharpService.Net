using SharpService.Components;
using SharpService.Configuration;
using System.Linq;

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
            return Get(configuration.protocolConfigurations.FirstOrDefault(x => x.Defalut).LoadBalance);
        }

        public ILoadBalanceProvider Get(string loadBalance)
        {
            switch (loadBalance)
            {
                case "random":
                    return ObjectContainer.ResolveNamed<ILoadBalanceProvider>(typeof(RandomLoadBalance).FullName);
                case "roundrobin":
                    return ObjectContainer.ResolveNamed<ILoadBalanceProvider>(typeof(RoundRobinLoadBalance).FullName);
                default:
                    throw new UnableToFindLoadBalanceException($"UnableToFindLoadBalance:{loadBalance}");
            }
        }
    }
}
