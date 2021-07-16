using ConsoleAppFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stazor.Commands;
using Stazor.Engines;
using Stazor.Engines.SimpleTextTemplateEngine;
using Stazor.Logging;
using Stazor.Plugins;
using Stazor.Plugins.Contents;
using Stazor.Plugins.IO;
using Stazor.Plugins.Metadata;
using Stazor.Themes;

await Host.CreateDefaultBuilder()
    .ConfigureServices(static (content, services) =>
    {
        // logging
        services.AddStazorLogging<BuildCommand>();
        services.AddStazorLogging<SimpleBlogPipeline>();

        // plugin
        // TODO: 設定値検証
        services.AddPlugin<ReadMarkdownFiles, ReadMarkdownFilesSettings>(content.Configuration.GetSection(ReadMarkdownFilesSettings.Key));
        services.AddPlugin<Breadcrumb, BreadcrumbSettings>(content.Configuration.GetSection(BreadcrumbSettings.Key));
        services.AddPlugin<Sort>();

        // theme
        services.AddSingleton<ITheme, Blog>();

        // engine
        services.AddSingleton<IEngine, Engine>();

        // pipeline
        services.AddSingleton<IPipeline, SimpleBlogPipeline>();

        // plugin resolver
        services.AddSingleton<IPluginResolver, PluginResolver>();
    })
    .ConfigureLogging(static logging => logging.AddStazorLogger())
    .RunConsoleAppFrameworkAsync<BuildCommand>(args)
    .ConfigureAwait(false);