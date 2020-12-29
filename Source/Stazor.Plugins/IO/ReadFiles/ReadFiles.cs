using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Stazor.Core;

namespace Stazor.Plugins.IO
{
    /// <summary>
    /// Reads the contents of the files.
    /// </summary>
    public sealed class ReadFiles : IPlugin
    {
        /// <summary>
        /// The content key.
        /// </summary>
        public static readonly byte[] Key = new byte[]
        {
            0x52, 0x65, 0x61, 0x64, 0x46, 0x69, 0x6C, 0x65, 0x73
        };

        readonly IStazorLogger _logger;
        readonly ReadFilesSettings _settings;
        readonly IEnumerable<string> _files;
        readonly string _templatePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadFiles"/> class.
        /// </summary>
        public ReadFiles(IStazorLogger logger, ReadFilesSettings settings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            _files = Directory.EnumerateFiles(_settings.Path, _settings.SearchPattern);
            _templatePath = _settings.TemplatePath!;
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<IDocument> ExecuteAsync(IAsyncEnumerable<IDocument> inputs)
        {
            await foreach (var input in inputs.ConfigureAwait(false))
            {
                yield return input;
            }

            foreach (var file in _files)
            {
                var document = DocumentFactory.GetDocument(_templatePath);
                document.Content.Add(Key, File.ReadAllBytes(file));

                yield return document;
            }
        }
    }
}