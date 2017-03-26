using System;
using System.Runtime.Serialization;

namespace SharpService.ServiceClients
{
    [Serializable]
    internal class UnableToFindServiceClientProviderException : Exception
    {
        public UnableToFindServiceClientProviderException()
        {
        }

        public UnableToFindServiceClientProviderException(string message) : base(message)
        {
        }

        public UnableToFindServiceClientProviderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnableToFindServiceClientProviderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}