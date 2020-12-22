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

        public void Info(string message)
        {
            _logger.ZLogInformation(message);
        }
    }
}