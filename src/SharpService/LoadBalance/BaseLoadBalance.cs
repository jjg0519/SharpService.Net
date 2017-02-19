using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpService.ServiceDiscovery;

namespace SharpService.LoadBalance
{
    public abstract class BaseLoadBalance : ILoadBalanceProvider
    {
        private List<RegistryInformation> services;

        public void OnRefresh(List<RegistryInformation> services)
        {
            this.services = services;
        }

        public RegistryInformation Select()
        {
            return Select(services);
        }

        public RegistryInformation Select(List<RegistryInformation> services)
        {
            if (services.Count() > 1)
            {
                return DoSelect(services);
            }
            else if (services.Count() == 1)
            {
                return services[0];
            }
            throw new Exception(" No available referers for call request:");
        }

        protected abstract RegistryInformation DoSelect(List<RegistryInformation> services);
    }
}

