using System;
using Microsoft.Extensions.DependencyInjection;

namespace Stazor
{
    public class ServiceActivator
    {
        internal static IServiceProvider? ServiceProvider { get; set; }

        public static void Configure(IServiceProvider serviceProvider)
            => ServiceProvider = serviceProvider;

        public static IServiceScope GetScope()
            => ServiceProvider?.GetRequiredService<IServiceScopeFactory>().CreateScope() ?? throw new InvalidOperationException(nameof(ServiceProvider));
    }
}