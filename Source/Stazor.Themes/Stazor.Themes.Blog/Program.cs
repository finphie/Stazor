﻿using ConsoleAppFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stazor.Commands;
using Stazor.Engines;
using Stazor.Engines.SimpleTextTemplateEngine;
using Stazor.Logging;
using Stazor.Themes;

await Host.CreateDefaultBuilder()
    .ConfigureServices(static (content, services) =>
    {
        services.AddSingleton<ITheme, Blog>();

        // services.AddStazorLogger<StazorLogger>
        // TODO: 設定値検証
        services.Configure<StazorSettings>(content.Configuration.GetSection(nameof(Stazor)));
        services.AddSingleton<IStazorLogger<BuildCommand>, StazorLogger<BuildCommand>>();
        services.AddSingleton<IEngine, Engine>();
        services.AddSingleton<IPipeline, SimpleBlogPipeline>();
    })
    .ConfigureLogging(static logging => logging.AddStazorLogger())
    .RunConsoleAppFrameworkAsync<BuildCommand>(args)
    .ConfigureAwait(false);