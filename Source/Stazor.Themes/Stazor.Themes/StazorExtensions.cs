using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stazor.Logging;
using Stazor.Plugins;
using Stazor.Themes.Helpers;

namespace Stazor.Themes
{
    /// <summary>
    /// <see cref="Stazor"/>関連の拡張メソッド
    /// </summary>
    public static class StazorExtensions
    {
        /// <summary>
        /// 設定クラスをコンテナに登録します。
        /// </summary>
        /// <typeparam name="T">設定の型</typeparam>
        /// <param name="services">サービスコンテナ</param>
        /// <param name="configuration">構成</param>
        /// <returns>設定クラスのオブジェクト</returns>
        public static T StazorConfigure<T>(this IServiceCollection services, IConfiguration configuration)
            where T : class, new()
        {
            var settings = configuration.Get<T>();
            var validationContext = new ValidationContext(settings, null, null);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(settings, validationContext, validationResults, true))
            {
                ThrowHelper.ThrowValidationException(validationResults);
            }

            services.AddSingleton(settings);
            return settings;
        }

        /// <summary>
        /// プラグインを追加します。
        /// </summary>
        /// <typeparam name="TPlugin">プラグインの型</typeparam>
        /// <param name="services">サービスコンテナ</param>
        public static void AddPlugin<TPlugin>(this IServiceCollection services)
            where TPlugin : class, IPlugin
        {
            services.AddStazorLogging<TPlugin>();
            services.AddSingleton<TPlugin>();
        }

        /// <summary>
        /// プラグインを追加します。
        /// </summary>
        /// <typeparam name="TPlugin">プラグインの型</typeparam>
        /// <typeparam name="TSettings">設定の型</typeparam>
        /// <param name="services">サービスコンテナ</param>
        /// <param name="configuration">構成</param>
        public static void AddPlugin<TPlugin, TSettings>(this IServiceCollection services, IConfiguration configuration)
            where TPlugin : class, IPlugin
            where TSettings : class, ISettingsKey, new()
        {
            services.AddPlugin<TPlugin>();
            services.StazorConfigure<TSettings>(configuration);
        }

        /// <summary>
        /// テーマを追加します。
        /// </summary>
        /// <typeparam name="TTheme">テーマの型</typeparam>
        /// <typeparam name="TSettings">設定の型</typeparam>
        /// <param name="services">サービスコンテナ</param>
        /// <param name="configuration">構成</param>
        public static void AddTheme<TTheme, TSettings>(this IServiceCollection services, IConfiguration configuration)
            where TTheme : class, ITheme
            where TSettings : class, ISettingsKey, new()
        {
            services.AddStazorLogging<TTheme>();
            services.AddSingleton<ITheme, TTheme>();
            services.StazorConfigure<TSettings>(configuration);
        }
    }
}