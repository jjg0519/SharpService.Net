using System;
using System.Runtime.Serialization;

namespace SharpService.LoadBalance
{
    [Serializable]
    internal class UnableToFindLoadBalanceException : Exception
    {
        public UnableToFindLoadBalanceException()
        {
        }

        public UnableToFindLoadBalanceException(string message) : base(message)
        {
        }

        public UnableToFindLoadBalanceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnableToFindLoadBalanceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}