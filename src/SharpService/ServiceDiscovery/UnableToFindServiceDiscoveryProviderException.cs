using System;
using System.Runtime.Serialization;

namespace SharpService.ServiceDiscovery
{
    [Serializable]
    internal class UnableToFindServiceDiscoveryProviderException : Exception
    {
        public UnableToFindServiceDiscoveryProviderException()
        {
        }

        public UnableToFindServiceDiscoveryProviderException(string message) : base(message)
        {
        }

        public UnableToFindServiceDiscoveryProviderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnableToFindServiceDiscoveryProviderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}