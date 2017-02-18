using System.Collections.Generic;

namespace SharpService.ServiceDiscovery
{
    public class RegistryInformation
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public string Version { get; set; }
        public List<string> Tags { get; set; }
    }
}
