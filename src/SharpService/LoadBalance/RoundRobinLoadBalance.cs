using System;
using System.Collections.Generic;
using System.Linq;
using SharpService.ServiceDiscovery;
using System.Threading;

namespace SharpService.LoadBalance
{
    public  class RoundRobinLoadBalance : BaseLoadBalance
    {
        private Random random = new Random();

        private int index = 0;

        protected override RegistryInformation DoSelect(List<RegistryInformation> services)
        {
            var i = random.Next(0, services.Count());
            Interlocked.Increment(ref index);
            return services[(i + index) % services.Count()];
        }
    }
}
