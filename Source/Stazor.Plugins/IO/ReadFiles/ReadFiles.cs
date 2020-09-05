using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Stazor.Core;

namespace Stazor.Plugins.IO
{
    public sealed class ReadFiles : IPlugin
    {
        readonly IEnumerable<string> _files;

        public ReadFiles(string path)
            : this(path, "*")
        {
        }

        public ReadFiles(string path, string searchPattern)
        {
            _files = Directory.EnumerateFiles(path, searchPattern);
        }

        public async IAsyncEnumerable<IDocument> ExecuteAsync(IAsyncEnumerable<IDocument> inputs)
        {
            await foreach (var input in inputs.ConfigureAwait(false))
            {
                yield return input;
            }

            foreach (var file in _files)
            {
                var document = DocumentFactory.GetDocument();
                document.Content.Add(nameof(ReadFiles), File.ReadAllBytes(file));

                yield return document;
            }
        }
    }
}