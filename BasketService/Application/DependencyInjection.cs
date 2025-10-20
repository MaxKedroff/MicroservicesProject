using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;


using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Services;
using StackExchange.Redis;
using Microsoft.EntityFrameworkCore;


using Domain.Interfaces;
using Microsoft.Extensions.Options;
using Infrastructure.Repositories;
using Infrastructure.Data;
using TraceLib;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IBasketLogService, BasketLogService>();

            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IBasketLogRepository, BasketLogsRepository>();
           
            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));

            services.AddDbContext<BasketLogDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));

            services.AddHttpClient("CatalogService", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5000");
            });

            return services;
        }
    }
}
