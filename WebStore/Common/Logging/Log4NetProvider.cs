using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace WebStore.Logging
{
    public class Log4NetProvider : ILoggerProvider
    {
        private readonly string _ConfigurationFile;
        private readonly ConcurrentDictionary<string, Log4NetLogger> _Loggers = new ConcurrentDictionary<string, Log4NetLogger>();
        public Log4NetProvider(string ConfigurationFile) => _ConfigurationFile = ConfigurationFile;

        public ILogger CreateLogger(string categoryName)
        {
            return _Loggers.GetOrAdd(categoryName, category =>
            {
                var xml = new XmlDocument();
                var file_name = _ConfigurationFile;
                xml.Load(file_name);
                return new Log4NetLogger(category, xml["log4net"]);
            });
        }

        public void Dispose()
        {
            _Loggers.Clear();
        }
    }
}
