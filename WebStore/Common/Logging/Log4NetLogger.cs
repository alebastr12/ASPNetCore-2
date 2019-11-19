using log4net;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;

namespace WebStore.Logging
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _Log;
        public Log4NetLogger(string CategoryName, XmlElement config)
        {
            var logger_repository = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy)
                );

            _Log = LogManager.GetLogger(logger_repository.Name, CategoryName);

            log4net.Config.XmlConfigurator.Configure(logger_repository, config);
        }
        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                default: throw new ArgumentOutOfRangeException(nameof(LogLevel), logLevel, null);

                case LogLevel.Trace:
                case LogLevel.Debug:
                    return _Log.IsDebugEnabled;
                case LogLevel.Information:
                    return _Log.IsInfoEnabled;
                case LogLevel.Warning:
                    return _Log.IsWarnEnabled;
                case LogLevel.Error:
                    return _Log.IsErrorEnabled;
                case LogLevel.Critical:
                    return _Log.IsFatalEnabled;
                case LogLevel.None:
                    return false;
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            if (formatter is null) throw new ArgumentNullException(nameof(formatter));

            var log_message = formatter(state, exception);

            if (string.IsNullOrEmpty(log_message) && exception is null) return;

            switch (logLevel)
            {
                default: throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);

                case LogLevel.Trace:
                case LogLevel.Debug:
                    _Log.Debug(log_message);
                    break;
                case LogLevel.Information:
                    _Log.Info(log_message);
                    break;
                case LogLevel.Warning:
                    _Log.Warn(log_message);
                    break;
                case LogLevel.Error:
                    _Log.Error(log_message ?? exception.ToString());
                    break;
                case LogLevel.Critical:
                    _Log.Fatal(log_message ?? exception.ToString());
                    break;
                case LogLevel.None:
                    break;
            }
        }
    }
}
