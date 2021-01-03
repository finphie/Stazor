using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ConsoleAppFramework;
using Cysharp.Text;
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
    class Program
    {
        [SuppressMessage("Style", "IDE1006:命名スタイル", Justification = "Entry-point")]
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .ConfigureServices(static (content, services) =>
                {
                    var themePath = content.Configuration["themePath"];
                    var themeName = content.Configuration["themeName"];

                    var loadContext = new LoadContext(themePath);
                    var assembly = loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(themePath)));
                    var types = assembly.GetTypes();
                    var type = types.First(static x => typeof(StazorBaseSettings).IsAssignableFrom(x));
                    var themeType = types.First(static x => typeof(ITheme).IsAssignableFrom(x));

                    services.Configure(type, content.Configuration.GetSection(StazorBaseSettings.Key));
                    services.AddSingleton(typeof(ITheme), themeType);
                    services.AddSingleton<IStazorLoggerFactory, StazorLoggerFactory>();
                    services.AddSingleton<IStazorLogger<BuildCommand>, StazorLogger<BuildCommand>>();
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

        [ModuleInitializer]
        public static void RegisterLogLevel()
        {
            Utf8ValueStringBuilder.RegisterTryFormat(static (LogLevel logLevel, Span<byte> destination, out int written, StandardFormat _) =>
            {
                // ログレベルを表す4文字のUTF-8文字列
                var value = logLevel switch
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
    }
}