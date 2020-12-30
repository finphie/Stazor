using System;
using Microsoft.Extensions.Logging;
using Stazor.Core;
using ZLogger;

namespace Stazor.Logging
{
    public sealed class StazorLogger<TCategoryName> : IStazorLogger<TCategoryName>
    {
        readonly ILogger _logger;

        public StazorLogger(ILogger<TCategoryName> logger)
            => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public void Trace(string message) => _logger.ZLogTrace(message);

        public void Debug(string message) => _logger.ZLogDebug(message);

        public void Information(string message) => _logger.ZLogInformation(message);

        public void Warning(string message) => _logger.ZLogWarning(message);

        public void Error(string message) => _logger.ZLogError(message);

        public void Critical(string message) => _logger.ZLogCritical(message);
    }
}