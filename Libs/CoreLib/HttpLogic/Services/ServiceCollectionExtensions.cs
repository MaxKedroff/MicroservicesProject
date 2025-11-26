using Core.HttpLogic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.HttpLogic.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpLogic(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.TryAddTransient<IHttpRequestService, HttpRequestService>();

            return services;
        }
    }
}
