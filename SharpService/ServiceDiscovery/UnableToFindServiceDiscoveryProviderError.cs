using System;

namespace SharpService.ServiceDiscovery
{
    public class UnableToFindServiceDiscoveryProviderException : Exception
    {
        public UnableToFindServiceDiscoveryProviderException(string message) : base(message)
        { }
    }
}
