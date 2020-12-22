using Microsoft.Extensions.Logging;
using Stazor.Core;
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
}