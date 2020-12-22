using System.Threading.Tasks;
using ConsoleAppFramework;
using Microsoft.Extensions.Logging;
using Stazor.Core;
using ZLogger;

namespace Stazor.Commands
{
    public sealed class BuildCommand : ConsoleAppBase
    {
        readonly ILogger<BuildCommand> _logger;
        readonly ITheme _theme;

        public BuildCommand(ILogger<BuildCommand> logger, ITheme theme)
        {
            _logger = logger;
            _theme = theme;
        }

        public async ValueTask Build(string themePath, string themeName, string[] args)
        {
            _logger.ZLogInformation("1");
            _logger.LogInformation("2");
            await _theme.ExecuteAsync().ConfigureAwait(false);
        }
    }
}
