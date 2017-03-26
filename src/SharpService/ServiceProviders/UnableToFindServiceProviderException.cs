using System;
using System.Runtime.Serialization;

namespace SharpService.ServiceProviders
{
    [Serializable]
    internal class UnableToFindServiceProviderException : Exception
    {
        public UnableToFindServiceProviderException()
        {
        }

        public UnableToFindServiceProviderException(string message) : base(message)
        {
        }

        public UnableToFindServiceProviderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnableToFindServiceProviderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}