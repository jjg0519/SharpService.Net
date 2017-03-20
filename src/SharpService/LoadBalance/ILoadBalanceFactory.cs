using SharpService.Configuration;

namespace SharpService.LoadBalance
{
    public interface ILoadBalanceFactory
    {
        ILoadBalanceProvider Get();

        ILoadBalanceProvider Get(ProtocolConfiguration protocolConfiguration);
    }
}
