using SharpService.Logging;
using NUnit.Framework;
using System;

namespace SharpService.Test
{
    public class LoggerTest
    {
        [Test]
        public void TestExceptionlessLogger()
        {
            ILogger logger = new ExceptionlessLogger();
            logger.LogDebug("debug");
            logger.LogError("error", new Exception("error"));
        }
    }
}
