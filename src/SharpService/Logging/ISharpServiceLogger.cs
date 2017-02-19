using Exceptionless;
using Exceptionless.Logging;
using System;

namespace SharpService.Logging
{
    public interface ISharpServiceLogger
    {
        void LogDebug(string message, params object[] args);
        void LogError(string message, Exception exception);
    }
}
