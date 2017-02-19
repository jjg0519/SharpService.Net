using SharpService.Components;
using SharpService.Configuration;
using System.Threading.Tasks;

namespace SharpService.LoadBalance
{
    public class LoadBalanceFactory : ILoadBalanceFactory
    {
        public Task<ILoadBalanceProvider> GetAsync(RefererConfiguration refererConfig)
        {
            switch (refererConfig.LoadBalance)
            {
                case "random":
                    return Task.FromResult(ObjectContainer.ResolveNamed<ILoadBalanceProvider>(
                        typeof(RandomLoadBalance).FullName));
                case "roundrobin":
                    return Task.FromResult(ObjectContainer.ResolveNamed<ILoadBalanceProvider>(
                        typeof(RoundRobinLoadBalance).FullName));
                default:
                    throw new UnableToFindLoadBalanceException("UnableToFindServiceDiscoveryProvider");
            }
        }
    }
}
