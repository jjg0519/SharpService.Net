using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpService.ServiceDiscovery
{
    public class InMemoryDiscoveryProvider : IServiceDiscoveryProvider
    {
        private static ConcurrentDictionary<string, ConcurrentDictionary<string, string>> memoryDic = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();


    }
}
