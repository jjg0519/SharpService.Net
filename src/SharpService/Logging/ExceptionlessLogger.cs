using Exceptionless;
using Exceptionless.Logging;
using System;

namespace SharpService.Logging
{
    public class ExceptionlessLogger : ILogger
    {
        static ExceptionlessLogger()
        {
            ExceptionlessClient.Default.Startup();
        }

        public void LogDebug(string message)
        {
            ExceptionlessClient.Default.CreateLog(message, LogLevel.Debug)
                                     .SetReferenceId(Guid.NewGuid().ToString("N"))
                                     .AddTags("NServiceDebug")
                                    .Submit(); ;
        }

        public void LogError(string message, Exception exception)
        {
            ExceptionlessClient.Default.CreateException(exception)
                                     .SetReferenceId(Guid.NewGuid().ToString("N"))
                                     .AddTags("NServiceError")
                                    .AddObject(message)
                                    .Submit();
        }
    }
}
