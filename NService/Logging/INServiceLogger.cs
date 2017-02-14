using System;

namespace NService.Logging
{
    public interface INServiceLogger
    {
        void LogDebug(string message, params object[] args);
        void LogError(string message, Exception exception);
    }
}
