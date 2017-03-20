using System;
using System.Collections.Generic;
using System.Linq;
using SharpService.ServiceDiscovery;
using System.Threading;

namespace SharpService.LoadBalance
{
    public  class RoundRobinLoadBalance : BaseLoadBalance
    {     
        private int index = 0;

        protected override RegistryInformation DoSelect(List<RegistryInformation> services)
        {
            Random random = new Random();
            var i = random.Next(0, services.Count());
            Interlocked.Increment(ref index);
            return services[(i + index) % services.Count()];
        }
    }
}
