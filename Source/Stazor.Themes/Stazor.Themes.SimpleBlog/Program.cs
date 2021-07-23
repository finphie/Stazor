using ConsoleAppFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stazor.Engines;
using Stazor.Logging;
using Stazor.Plugins;
using Stazor.Plugins.Contents;
using Stazor.Plugins.IO;
using Stazor.Plugins.Metadata;
using Stazor.Themes;
using Stazor.Themes.Commands;

await Host.CreateDefaultBuilder()
    .ConfigureServices(static (content, services) =>
    {
        // logging
        services.AddStazorLogging<App>();
        services.AddStazorLogging<SimpleBlogPipeline>();

        // plugin
        // TODO: 設定値検証
        services.AddPlugin<ReadMarkdownFiles, ReadMarkdownFilesSettings>(content.Configuration.GetSection(ReadMarkdownFilesSettings.Key));
        services.AddPlugin<Breadcrumb, BreadcrumbSettings>(content.Configuration.GetSection(BreadcrumbSettings.Key));
        services.AddPlugin<Sort>();

        // theme
        services.AddTheme<SimpleBlog, SimpleBlogSettings>(content.Configuration.GetSection(SimpleBlogSettings.Key));

        // engine
        services.AddSingleton<IEngine, SimpleTextTemplateEngine>();

        // pipeline
        services.AddSingleton<IPipeline, SimpleBlogPipeline>();

        // plugin resolver
        services.AddSingleton<IPluginResolver, PluginResolver>();
    })
    .ConfigureLogging(static logging => logging.AddStazorLogger())
    .RunConsoleAppFrameworkAsync<App>(args)
    .ConfigureAwait(false);