using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using System;
using System.IO;

namespace SharpService.Logging
{
    public class Log4NetLogger : ILogger
    {

        private readonly ILog _log;

        public Log4NetLogger(string configFile)
        {
            var file = new FileInfo(configFile);
            if (!file.Exists)
            {
                file = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile));
            }

            if (file.Exists)
            {
                XmlConfigurator.ConfigureAndWatch(file);
            }
            else
            {
                BasicConfigurator.Configure(new ConsoleAppender { Layout = new PatternLayout() });
            }

            _log= LogManager.GetLogger(this.GetType().FullName);
        }

        public void LogDebug(string message)
        {
            _log.Debug(message);
        }

        public void LogError(string message, Exception exception)
        {
            _log.Error(message, exception);
        }
    }
}
