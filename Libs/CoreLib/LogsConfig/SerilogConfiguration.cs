using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Events;

namespace CoreLib.Logs
{
    public static class SerilogConfiguration
    {
        /// <summary>
        /// Добавление логирования
        /// </summary>
        public static IServiceCollection AddLoggerServices(this IServiceCollection services)
        {
            return services
                .AddSingleton(Log.Logger);
        }

        /// <summary>
        /// Получить конфигурацию
        /// </summary>
        public static LoggerConfiguration GetConfiguration(this LoggerConfiguration loggerConfiguration)
        {
            var logFormat = "{Timestamp:HH:mm:ss:ms} LEVEL:[{Level}] TRACE:|{TraceId}| THREAD:|{ThreadId}|{TenantId} {Message}{NewLine}{Exception}";

            return loggerConfiguration
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Information)
                .MinimumLevel.Is(LogEventLevel.Information)
                .Enrich.WithThreadId() // добавляет TraceId
                .Enrich.FromLogContext()
                .WriteTo.Async(option =>
                {
                    option.Console(LogEventLevel.Information, outputTemplate: logFormat);
                });
        }
    }
}
