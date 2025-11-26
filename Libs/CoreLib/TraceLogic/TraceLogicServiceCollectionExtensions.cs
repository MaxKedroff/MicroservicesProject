using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceLib;

namespace CoreLib.TraceLogic
{
    public static class TraceLogicServiceCollectionExtensions
    {
        public static IServiceCollection AddTraceId(this IServiceCollection services) // ✅ ПРАВИЛЬНО
        {
            services.TryAddScoped<ITraceIdAccessor, TraceIdAccessor>();
            return services;
        }
    }
}
