using System;

namespace SharpService.Logging
{
    public interface ILogger
    {
        void LogDebug(string message);
        void LogError(string message, Exception exception);
    }
}
