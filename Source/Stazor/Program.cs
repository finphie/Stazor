using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ConsoleAppFramework;
using Cysharp.Text;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Stazor.Commands;
using Stazor.Core;
using Stazor.Extensions;
using Stazor.Logging;
using ZLogger;

namespace Stazor
{
    /// <summary>
    /// The Main class of the application.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// ログレベルの解析メソッドを登録
        /// </summary>
        [ModuleInitializer]
        public static void RegisterLogLevel()
        {
            Utf8ValueStringBuilder.RegisterTryFormat(static (LogLevel logLevel, Span<byte> destination, out int written, StandardFormat _) =>
            {
                // ログレベルを表す4文字のUTF-8文字列
#pragma warning disable IDE0072 // 欠落しているケースの追加
                var value = logLevel switch
#pragma warning restore IDE0072 // 欠落しているケースの追加
                {
                    // trce
                    LogLevel.Trace => 0x65637274,

                    // dbug
                    LogLevel.Debug => 0x67756264,

                    // info
                    LogLevel.Information => 0x6f666e69,

                    // warn
                    LogLevel.Warning => 0x6e726177,

                    // fail
                    LogLevel.Error => 0x6c696166,

                    // crit
                    LogLevel.Critical => 0x74697263,

                    // LogLevel.Noneや未知のログレベルの場合
                    _ => throw new ArgumentOutOfRangeException(nameof(logLevel))
                };

                ref var destinationStart = ref MemoryMarshal.GetReference(destination);
                Unsafe.WriteUnaligned(ref destinationStart, value);
                written = 4;

                return true;
            });
        }

        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .ConfigureServices(static (content, services) =>
                {
                    var themePath = content.Configuration["themePath"];
                    var themeName = content.Configuration["themeName"];

                    //var loadContext = new LoadContext(themePath);
                    //var assembly = loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(themePath)));
                    //var types = assembly.GetTypes();
                    //var type = types.First(x => x.BaseType == typeof(StazorBaseSettings));
                    //var themeType = types.First(typeof(ITheme).IsAssignableFrom);

                    var plugin = PluginLoader.CreateFromAssemblyFile(themePath, new[] { typeof(StazorBaseSettings) });
                    var types = plugin.LoadDefaultAssembly().GetTypes();
                    var pluginType = types.First(t => typeof(StazorBaseSettings).IsAssignableFrom(t) && !t.IsAbstract);
                    var themeType = types.First(typeof(ITheme).IsAssignableFrom);
                    services.Configure(pluginType, content.Configuration.GetSection(nameof(Stazor)));

                    //services.Configure(type, content.Configuration.GetSection(nameof(Stazor)));
                    services.AddSingleton(typeof(ITheme), themeType);
                    services.AddSingleton<IStazorLoggerFactory, StazorLoggerFactory>();
                    services.AddSingleton<IStazorLogger<BuildCommand>, StazorLogger<BuildCommand>>();
                    // services.AddPluginFromAssemblyFile
                })
                .ConfigureLogging(static logging =>
                {
                    logging.ClearProviders();
                    logging.AddZLoggerConsole(static options =>
                    {
                        var prefixFormat = ZString.PrepareUtf8<DateTime, LogLevel, string>("{0:O} [{1}] {2} - ");

                        options.PrefixFormatter = (writer, info) =>
                            prefixFormat.FormatTo(ref writer, info.Timestamp.UtcDateTime, info.LogLevel, info.CategoryName);
                    });
                })
                .RunConsoleAppFrameworkAsync<BuildCommand>(args)
                .ConfigureAwait(false);
        }
    }
}