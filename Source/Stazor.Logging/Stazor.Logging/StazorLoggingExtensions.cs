using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace Stazor.Logging
{
    public static class StazorLoggingExtensions
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

        public static void AddStazorLogger<T>(this IServiceCollection services)
        {
            services.AddSingleton<IStazorLoggerFactory, StazorLoggerFactory>();
            services.AddSingleton<IStazorLogger<T>, StazorLogger<T>>();
        }

        public static void AddStazorLogger(this ILoggingBuilder logging)
        {
            logging.ClearProviders();
            logging.AddZLoggerConsole(static options =>
            {
                var prefixFormat = ZString.PrepareUtf8<DateTime, LogLevel, string>("{0:O} [{1}] {2} - ");

                options.PrefixFormatter = (writer, info) =>
                    prefixFormat.FormatTo(ref writer, info.Timestamp.UtcDateTime, info.LogLevel, info.CategoryName);
            });
        }
    }
}