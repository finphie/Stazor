﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stazor.Logging;
using Stazor.Plugins;

namespace Stazor.Themes
{
    public static class StazorExtensions
    {
        public static T StazorConfigure<T>(this IServiceCollection services, IConfiguration configuration)
            where T : class, new()
        {
            var settings = new T();
            configuration.Bind(settings);
            services.AddSingleton(settings);
            return settings;
        }

        public static void AddPlugin<TPlugin>(this IServiceCollection services)
            where TPlugin : class, IPlugin
        {
            services.AddStazorLogging<TPlugin>();
            services.AddSingleton<TPlugin>();
        }

        public static void AddPlugin<TPlugin, TSettings>(this IServiceCollection services, IConfiguration configuration)
            where TPlugin : class, IPlugin
            where TSettings : class, ISettingsKey, new()
        {
            services.AddPlugin<TPlugin>();
            services.StazorConfigure<TSettings>(configuration);
        }

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