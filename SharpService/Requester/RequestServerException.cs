using System;

namespace SharpService.Requester
{
    public class RequestServerException : Exception
    {
        public RequestServerException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
