using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Stazor.Extensions
{
    static class ServiceCollectionExtensions
    {
        public static void Configure(this IServiceCollection services, Type type, IConfiguration configuration)
        {
            // nullにはならないはず
            var settings = Activator.CreateInstance(type)!;

            configuration.Bind(settings);
            services.AddSingleton(type, settings);
        }
    }
}
