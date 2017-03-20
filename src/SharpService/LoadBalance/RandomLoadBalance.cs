using System;
using System.Collections.Generic;
using System.Linq;
using SharpService.ServiceDiscovery;

namespace SharpService.LoadBalance
{
    public class RandomLoadBalance : BaseLoadBalance
    {
        protected override RegistryInformation DoSelect(List<RegistryInformation> services)
        {
            Random random = new Random();
            return services[random.Next(0, services.Count())];
        }
    }
}
