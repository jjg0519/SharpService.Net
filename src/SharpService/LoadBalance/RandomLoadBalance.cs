using System;
using System.Collections.Generic;
using System.Linq;
using SharpService.ServiceDiscovery;

namespace SharpService.LoadBalance
{
    public class RandomLoadBalance : BaseLoadBalance
    {
        private  Random random= new Random();

        protected override RegistryInformation DoSelect(List<RegistryInformation> services)
        {
            return services[random.Next(0, services.Count())];
        }
    }
}
