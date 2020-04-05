using System.Collections.Generic;
using System.IO;

namespace Stazor.Core.Plugins
{
    public sealed class ReadFiles : IPlugin
    {
        IEnumerable<string> _files;

        public ReadFiles(string path)
            : this(path, "*")
        {
        }

        public ReadFiles(string path, string searchPattern)
        {
            _files = Directory.EnumerateFiles(path, searchPattern, SearchOption.AllDirectories);
        }

        public async IAsyncEnumerable<IDocument> ExecuteAsync(IAsyncEnumerable<IDocument> inputs)
        {
            await foreach (var input in inputs)
            {
                yield return input;
            }

            foreach (var file in _files)
            {
                yield return DocumentFactory.GetDocument(File.ReadAllText(file));
            }
        }
    }
}