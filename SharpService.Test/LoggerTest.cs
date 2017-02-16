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
            INServiceLogger logger = new ExceptionlessLogger();
            logger.LogDebug("debug", "arg1", "arg2");
            logger.LogError("error", new Exception("error"));
        }
    }
}
