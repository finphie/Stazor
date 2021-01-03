using Microsoft.Extensions.Logging;
using Stazor.Core;

namespace Stazor.Logging
{
    public sealed class StazorLoggerFactory : IStazorLoggerFactory
    {
        readonly ILoggerFactory _loggerFactory;

        public StazorLoggerFactory(ILoggerFactory loggerFactory)
            => _loggerFactory = loggerFactory;

        public IStazorLogger CreateLogger<TCategoryName>()
            => new StazorLogger<TCategoryName>(_loggerFactory.CreateLogger<TCategoryName>());
    }
}