using System;

namespace SharpService.Requester
{
    public class RequestException : Exception
    {
        public RequestException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
