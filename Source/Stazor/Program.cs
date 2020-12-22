using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ConsoleAppFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stazor.Commands;
using Stazor.Core;
using Stazor.Extensions;
using ZLogger;

namespace Stazor
{
    public sealed class StazorLogger : IStazorLogger
    {
        readonly ILogger _logger;

        public StazorLogger(ILogger<StazorLogger> logger)
        {
            _logger = logger;
        }


        public void Info(string message)
        {
            _logger.ZLogInformation(message);
        }
    }

    public sealed class TeS : StazorBaseSettings { }

    /// <summary>
    /// The Main class of the application.
    /// </summary>
    [SuppressMessage("Performance", "CA1822:メンバーを static に設定します", Justification = "Command-line application")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Entry-point")]
    class Program : ConsoleAppBase
    {
        [SuppressMessage("Style", "IDE1006:命名スタイル", Justification = "Entry-point")]
        static async Task Main(string[] args)
        {
            // TODO: 設定ファイル検証

            var host = Host.CreateDefaultBuilder() //args
                .ConfigureAppConfiguration((_, x) => x.AddCommandLine(args))
                .ConfigureServices(static (content, services) =>
                {
                    var themePath = content.Configuration["themePath"];
                    var themeName = content.Configuration["themeName"];

                    var loadContext = new LoadContext(themePath);
                    var assembly = loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(themePath)));
                    var types = assembly.GetTypes();
                    var type = types.First(x => typeof(StazorBaseSettings).IsAssignableFrom(x));
                    var themeType = types.First(x => typeof(ITheme).IsAssignableFrom(x));

                    services.Configure(content.Configuration, type);
                    // services.Configure<TeS>(content.Configuration);

                    //services.AddSingleton(type, provider =>
                    //{
                    //    // IOptions<ITheme>型を作成
                    //    var options = typeof(IOptions<>).MakeGenericType(type);

                    //    var aaa = provider.GetRequiredService(options);
                    //    var bbb = provider.GetRequiredService(typeof(IOptions<TeS>));

                    //    // IOptions<ITheme>型のインスタンスを返す。取得がnullになることはないはず。
                    //    // また、テーマはIOptions<>に依存させたくないため、直接設定をインジェクションする。
                    //    return (provider.GetRequiredService(options) as IOptions<ISettings>)!.Value;
                    //});

                    // services.AddSingleton(type);


                    //var themeType = GetT(themePath, themeName);

                    services.AddSingleton(typeof(ITheme), themeType);
                    services.AddSingleton<IStazorLogger, StazorLogger>();
                })
                .ConfigureLogging(static logging =>
                {
                    logging.ClearProviders();
                    logging.AddZLoggerConsole();
                })
                .UseConsoleAppFramework<BuildCommand>(args)
                //.UseConsoleAppFramework<Program>(args)
                .Build();

            loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
            //settings = host.Services.GetRequiredService<ISettings>(); //(type) as ISettings;

            await host.RunAsync().ConfigureAwait(false);
        }

        static ILoggerFactory loggerFactory;
        static ISettings settings;

        //[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Command-line application")]
        //public async Task BuildAsync(string themePath, string themeName, string[] args)
        //{
        //    var theme = CreateTheme(themePath, themeName);

        //    await theme.ExecuteAsync().ConfigureAwait(false);
        //}

        static ITheme CreateTheme(string themePath, string themeName, params object[] args)
        {
            var loadContext = new LoadContext(themePath);
            var assembly = loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(themePath)));

            foreach (var type in assembly.GetTypes())
            {
                if (!typeof(ITheme).IsAssignableFrom(type) || !type.Name.Equals(themeName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (Activator.CreateInstance(type, loggerFactory, settings) is ITheme result)
                {
                    return result;
                }
            }

            throw new ArgumentException($"Can't find any type which implements {nameof(ITheme)} in {assembly} from {assembly.Location}.");
        }

        static Type GetT(string themePath, string themeName)
        {
            var loadContext = new LoadContext(themePath);
            var assembly = loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(themePath)));

            foreach (var type in assembly.GetTypes())
            {
                if (!typeof(ITheme).IsAssignableFrom(type) || !type.Name.Equals(themeName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                return type;
            }

            throw new ArgumentException($"Can't find any type which implements {nameof(ITheme)} in {assembly} from {assembly.Location}.");
        }
    }
}
