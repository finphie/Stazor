using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Stazor.Extensions
{
    static class ServiceCollectionExtensions
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration, Type type)
        {
            var settings = Activator.CreateInstance(type);
            configuration.Bind(settings);
            services.AddSingleton(type, settings);
        }
    }
}
