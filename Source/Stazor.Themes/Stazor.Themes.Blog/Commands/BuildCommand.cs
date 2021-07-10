using System.Threading.Tasks;
using ConsoleAppFramework;
using Stazor.Core;
using Stazor.Logging;
using Stazor.Themes;

namespace Stazor.Commands
{
    public sealed class BuildCommand : ConsoleAppBase
    {
        readonly IStazorLogger _logger;
        readonly ITheme _theme;

        public BuildCommand(IStazorLogger<BuildCommand> logger, ITheme theme)
        {
            _logger = logger;
            _theme = theme;
        }

        public async ValueTask Build(string themePath, string themeName, string[] args)
        {
            _logger.Information("Start");

            await _theme.ExecuteAsync().ConfigureAwait(false);

            _logger.Information("End");
        }
    }
}