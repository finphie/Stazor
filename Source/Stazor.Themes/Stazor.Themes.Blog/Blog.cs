using System;
using System.Buffers;
using System.IO;
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

            Pipeline = new(new ReadMarkdownFiles(CreateLogger<ReadMarkdownFiles>(), _settings.Markdown));
            //Pipeline.Add(new Sort(CreateLogger<Sort>()));
            Pipeline.Add(new Breadcrumb(CreateLogger<Breadcrumb>(), _settings.Breadcrumb));

            IStazorLogger CreateLogger<T>() => _loggerFactory.CreateLogger<T>();
        }

        /// <inheritdoc/>
        public IEngine Engine => SimpleEngine.Default;

        /// <inheritdoc/>
        public Pipeline Pipeline { get; }

        /// <inheritdoc/>
        public async ValueTask ExecuteAsync()
        {
            var filePaths = Directory.GetFiles(_settings.ContentPath, "*.md");
            var buffer = new ArrayBufferWriter<byte>();
            var documents = Pipeline.Execute(filePaths);

            var i = 0;
            for (i = 0; i < documents.Length; i++)
            {
                await Engine.ExecuteAsync(buffer, documents[i]).ConfigureAwait(false);

                Console.Write(Encoding.UTF8.GetString(buffer.WrittenSpan).Length + ",");

                //using var fs = new FileStream($"g/{i}.html", FileMode.Create, FileAccess.Write, FileShare.Read);
                //fs.Write(buffer.WrittenSpan);
                buffer.Clear();
            }

            Console.WriteLine();
            Console.WriteLine(i);
        }
    }
}