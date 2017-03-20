using SharpService.ServiceDiscovery;
using System;
using System.Runtime.Serialization;

namespace SharpService.ServiceRequester
{
    [Serializable]
    internal class UnableRequestException : Exception
    {
        public RegistryInformation service { set; get; }

        public UnableRequestException()
        {
        }

        public UnableRequestException(string message) : base(message)
        {
        }

        public UnableRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnableRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}