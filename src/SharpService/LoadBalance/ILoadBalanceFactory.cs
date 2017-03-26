namespace SharpService.LoadBalance
{
    public interface ILoadBalanceFactory
    {
        ILoadBalanceProvider Get();

        ILoadBalanceProvider Get(string loadBalance);
    }
}
