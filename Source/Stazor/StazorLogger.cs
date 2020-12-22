using System;
using Microsoft.Extensions.Logging;
using Stazor.Core;
using ZLogger;

namespace Stazor
{
    public sealed class StazorLogger<TCategoryName> : IStazorLogger<TCategoryName>
    {
        readonly ILogger _logger;

        public StazorLogger(ILogger<TCategoryName> logger)
            => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public void Info(string message)
        {
            _logger.ZLogInformation(message);
        }
    }

    public sealed class StazorLoggerFactory : IStazorLoggerFactory
    {
        readonly ILoggerFactory _loggerFactory;

        public StazorLoggerFactory(ILoggerFactory loggerFactory)
            => _loggerFactory = loggerFactory;

        public IStazorLogger CreateLogger<TCategory>()
            => new StazorLogger<TCategory>(_loggerFactory.CreateLogger<TCategory>());
    }
}