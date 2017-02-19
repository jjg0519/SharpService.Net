using SharpService.ServiceDiscovery;
using System.Collections.Generic;

namespace SharpService.LoadBalance
{
    public interface ILoadBalanceProvider
    {
        void OnRefresh(List<RegistryInformation> services);

        RegistryInformation Select();

        RegistryInformation Select(List<RegistryInformation> services);
    }
}
