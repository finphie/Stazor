using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ConsoleAppFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Stazor.Commands;
using Stazor.Core;
using Stazor.Extensions;
using Stazor.Logging;
using ZLogger;

namespace Stazor
{
    /// <summary>
    /// The Main class of the application.
    /// </summary>
    class Program
    {
        [SuppressMessage("Style", "IDE1006:命名スタイル", Justification = "Entry-point")]
        static async Task Main(string[] args)
        {
            // TODO: 設定ファイル検証

            await Host.CreateDefaultBuilder(args)
                .ConfigureServices(static (content, services) =>
                {
                    var themePath = content.Configuration["themePath"];
                    var themeName = content.Configuration["themeName"];

                    var loadContext = new LoadContext(themePath);
                    var assembly = loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(themePath)));
                    var types = assembly.GetTypes();
                    var type = types.First(x => typeof(StazorBaseSettings).IsAssignableFrom(x));
                    var themeType = types.First(x => typeof(ITheme).IsAssignableFrom(x));

                    services.Configure(type, content.Configuration.GetSection(StazorBaseSettings.Key));
                    services.AddSingleton(typeof(ITheme), themeType);
                    services.AddSingleton<IStazorLoggerFactory, StazorLoggerFactory>();
                })
                .ConfigureLogging(static logging =>
                {
                    logging.ClearProviders();
                    logging.AddZLoggerConsole();
                })
                .RunConsoleAppFrameworkAsync<BuildCommand>(args)
                .ConfigureAwait(false);
        }
    }
}