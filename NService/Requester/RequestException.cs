using System;

namespace NService.Requester
{
    public class RequestException : Exception
    {
        public RequestException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
