using System;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace Stazor.Logging
{
    /// <summary>
    /// Stazor専用のロガークラスです。
    /// </summary>
    /// <typeparam name="TCategoryName">カテゴリー名</typeparam>
    public sealed class StazorLogger<TCategoryName> : IStazorLogger<TCategoryName>
    {
        readonly ILogger _logger;

        /// <summary>
        /// <see cref="StazorLogger{TCategoryName}"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="logger">ロガー</param>
        public StazorLogger(ILogger<TCategoryName> logger)
            => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <inheritdoc/>
        public void Trace(string message) => _logger.ZLogTrace(message);

        /// <inheritdoc/>
        public void Debug(string message) => _logger.ZLogDebug(message);

        /// <inheritdoc/>
        public void Information(string message) => _logger.ZLogInformation(message);

        /// <inheritdoc/>
        public void Warning(string message) => _logger.ZLogWarning(message);

        /// <inheritdoc/>
        public void Error(string message) => _logger.ZLogError(message);

        /// <inheritdoc/>
        public void Critical(string message) => _logger.ZLogCritical(message);
    }
}