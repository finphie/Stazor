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

        readonly IEnumerable<string> _files;
        readonly string _templatePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadFiles"/> class.
        /// </summary>
        /// <param name="path">The relative or absolute path to the directory to search.</param>
        /// <param name="templatePath">The relative or absolute path to the template directory.</param>
        public ReadFiles(string path, string templatePath)
            : this(path, "*", templatePath)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadFiles"/> class.
        /// </summary>
        /// <param name="path">The relative or absolute path to the directory to search.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path.</param>
        /// <param name="templatePath">The relative or absolute path to the template directory.</param>
        public ReadFiles(string path, string searchPattern, string templatePath)
        {
            _files = Directory.EnumerateFiles(path, searchPattern);
            _templatePath = templatePath;
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