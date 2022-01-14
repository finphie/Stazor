using Microsoft.Extensions.DependencyInjection;
using Stazor.Engines;
using Stazor.Logging;
using Stazor.Plugins;
using Stazor.Plugins.Contents;
using Stazor.Plugins.IO;
using Stazor.Plugins.Metadata;
using Stazor.Themes;

var builder = ConsoleApp.CreateBuilder(args)
    .ConfigureServices(static (content, services) =>
    {
        // logging
        services.AddStazorLogging<SimpleBlogPipeline>();

        // plugin
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
    .ConfigureLogging(static logging => logging.AddStazorLogger());

var app = builder.Build();

app.AddCommand("build", static async (ITheme theme) => await theme.ExecuteAsync().ConfigureAwait(false));

app.Run();
