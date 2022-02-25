using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace Stazor.Logging;

/// <summary>
/// Stazor専用ロガーに関する拡張メソッドです。
/// </summary>
public static class StazorLoggingExtensions
{
    /// <summary>
    /// ログレベルの解析メソッドを登録
    /// </summary>
    [ModuleInitializer]
    [SuppressMessage("Usage", "CA2255:The 'ModuleInitializer' attribute should not be used in libraries", Justification = "無効化しても問題ないはず")]
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

    /// <summary>
    /// Stazor専用ロギング処理を追加します。
    /// </summary>
    /// <typeparam name="T">対象の型</typeparam>
    /// <param name="services">サービスコンテナ</param>
    public static void AddStazorLogging<T>(this IServiceCollection services!!)
        => services.AddSingleton<IStazorLogger<T>, StazorLogger<T>>();

    /// <summary>
    /// Stazor専用ロガーを追加します。
    /// </summary>
    /// <param name="logging">ログビルダー</param>
    public static void AddStazorLogger(this ILoggingBuilder logging!!)
    {
        logging.ClearProviders();
        logging.AddZLoggerConsole(static options =>
        {
            var prefixFormat = ZString.PrepareUtf8<DateTime, int, LogLevel, string>("{0:O},{1} [{2}] {3} - ");

            options.PrefixFormatter = (writer, info) =>
                prefixFormat.FormatTo(ref writer, info.Timestamp.UtcDateTime, Environment.CurrentManagedThreadId, info.LogLevel, info.CategoryName);
        });
    }
}
