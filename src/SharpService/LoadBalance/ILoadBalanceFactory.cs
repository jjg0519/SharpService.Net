using SharpService.Configuration;
using System.Threading.Tasks;

namespace SharpService.LoadBalance
{
    public interface ILoadBalanceFactory
    {
        Task<ILoadBalanceProvider> GetAsync(RefererConfiguration refererConfig);
    }
}
