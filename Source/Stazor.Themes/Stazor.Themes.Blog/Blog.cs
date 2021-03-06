﻿using System;
using System.Buffers;
using System.Text;
using System.Threading.Tasks;
using Stazor.Core;
using Stazor.Engine;
using Stazor.Engines.Simple;
using Stazor.Plugins.Contents;
using Stazor.Plugins.IO;
using Stazor.Plugins.Metadata;

namespace Stazor.Themes
{
    /// <summary>
    /// The simple blog template.
    /// </summary>
    public sealed class Blog : ITheme
    {
        readonly IStazorLoggerFactory _loggerFactory;
        readonly StazorSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="Blog"/> class.
        /// </summary>
        public Blog(IStazorLoggerFactory loggerFactory, StazorSettings settings)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            Pipeline.Add(new ReadMarkdownFiles(CreateLogger<ReadMarkdownFiles>(), _settings.Markdown));
            Pipeline.Add(new Sort(CreateLogger<Sort>()));
            Pipeline.Add(new Breadcrumb(CreateLogger<Breadcrumb>(), _settings.Breadcrumb));

            IStazorLogger CreateLogger<T>() => _loggerFactory.CreateLogger<T>();
        }

        /// <inheritdoc/>
        public IEngine Engine => SimpleEngine.Default;

        /// <inheritdoc/>
        public Pipeline Pipeline { get; } = new();

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync()
        {
            var buffer = new ArrayBufferWriter<byte>();

            // var count = 0;
            await foreach (var document in Pipeline.ExecuteAsync().ConfigureAwait(false))
            {
                await Engine.ExecuteAsync(buffer, document).ConfigureAwait(false);

                Console.WriteLine(Encoding.UTF8.GetString(buffer.WrittenSpan));

                // using var fs = new FileStream($"{count++}.html", FileMode.Create, FileAccess.Write, FileShare.Read);
                // fs.Write(buffer.WrittenSpan);
                buffer.Clear();
            }
        }
    }
}