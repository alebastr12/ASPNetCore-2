using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace WebStore.Logging
{
    public static class Log4NetExstention
    {
        public static ILoggerFactory AddLog4Net(this ILoggerFactory Factory, string ConfigurationFile = "log4net.config")
        {
            if (!Path.IsPathRooted(ConfigurationFile))
            {
                var assembly = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("Не найдена сборка с точкой входа в приложение!");
                var dir = Path.GetDirectoryName(assembly.Location) ?? throw new InvalidOperationException("Рабочий каталог приложения установить не удалось");
                ConfigurationFile = Path.Combine(dir, ConfigurationFile);
            }

            Factory.AddProvider(new Log4NetProvider(ConfigurationFile));

            return Factory;
        }
    }
}
