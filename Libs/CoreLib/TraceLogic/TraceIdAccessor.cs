using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceLib.Interfaces;

namespace TraceLib
{

    public interface ITraceIdAccessor
    {
        string GetValue();
    }

    public static class StartUpTraceId
    {
        public static IServiceCollection TryAddTraceId(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<TraceIdAccessor>();
            serviceCollection
                .TryAddScoped<ITraceWriter>(provider => provider.GetRequiredService<TraceIdAccessor>());
            serviceCollection
                .TryAddScoped<ITraceReader>(provider => provider.GetRequiredService<TraceIdAccessor>());
            serviceCollection
                .TryAddScoped<ITraceIdAccessor>(provider => provider.GetRequiredService<TraceIdAccessor>());

            return serviceCollection;
        }
    }

    public class TraceIdAccessor : ITraceReader, ITraceWriter, ITraceIdAccessor
    {
        public string Name => "TraceId";

        private string _value;

        public string GetValue()
        {
            return _value;
        }

        public void WriteValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                value = Guid.NewGuid().ToString();
            }

            _value = value;
            LogContext.PushProperty("TraceId", value);

        }
    }
}
