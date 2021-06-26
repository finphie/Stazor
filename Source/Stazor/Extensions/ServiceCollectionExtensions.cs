using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stazor.Core;

namespace Stazor.Extensions
{
    /// <summary>
    /// <see cref="IServiceCollection"/>の拡張メソッド
    /// </summary>
    static class ServiceCollectionExtensions
    {
        public static void Configure(this IServiceCollection services, Type type, IConfiguration configuration)
        {
            // nullにはならないはず
            var settings = Activator.CreateInstance(type)!;

            configuration.Bind(settings);

            // 設定値を検証する。
            if (settings is IValidatable value)
            {
                value.Validate();
            }

            services.AddSingleton(type, settings);
        }
    }
}