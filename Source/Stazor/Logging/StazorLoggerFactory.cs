using Microsoft.Extensions.Logging;
using Stazor.Core;

namespace Stazor.Logging
{
    /// <summary>
    /// Stazor専用ロガーのファクトリー
    /// </summary>
    public sealed class StazorLoggerFactory : IStazorLoggerFactory
    {
        readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// <see cref="StazorLoggerFactory"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="loggerFactory">ロガーファクトリー</param>
        public StazorLoggerFactory(ILoggerFactory loggerFactory)
            => _loggerFactory = loggerFactory;

        /// <inheritdoc/>
        public IStazorLogger CreateLogger<TCategoryName>()
            => new StazorLogger<TCategoryName>(_loggerFactory.CreateLogger<TCategoryName>());
    }
}