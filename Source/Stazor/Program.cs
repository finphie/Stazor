using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using ConsoleAppFramework;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Stazor.Core.Themes;
using ZLogger;

namespace Stazor
{
    class Program : ConsoleAppBase
    {
        [SuppressMessage("Style", "IDE1006:命名スタイル", Justification = "Entry-point")]
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder()
                .ConfigureServices((content, services) =>
                {
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddZLoggerConsole();
                })
                .RunConsoleAppFrameworkAsync<Program>(args);
        }

        // [Command("build")]
        public async Task Build(string themePath, string themeName, string[] args)
        {
            var theme = CreateTheme(themePath, themeName, args);           

            await theme.ExecuteAsync();
        }

        static ITheme CreateTheme(string themePath, string themeName, params string[] args)
        {
            var loadContext = new LoadContext(themePath);
            var assembly = loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(themePath)));

            foreach (var type in assembly.GetTypes())
            {
                if (!typeof(ITheme).IsAssignableFrom(type) || !type.Name.Equals(themeName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (Activator.CreateInstance(type, args) is ITheme result)
                {
                    return result;
                }
            }

            throw new ArgumentException($"Can't find any type which implements {nameof(ITheme)} in {assembly} from {assembly.Location}.");
        }
    }
}