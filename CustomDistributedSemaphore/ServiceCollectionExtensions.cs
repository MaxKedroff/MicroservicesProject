using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomDistributedSemaphore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomDistributedSemaphore(this IServiceCollection services, string redisConnectionString)
        {
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(redisConnectionString));

            services.AddScoped<IDistributedSemaphoreFactory, DistributedSemaphoreFactory>();
            return services;
        }
    }
}
