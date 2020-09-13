using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Stazor.Core;

namespace Stazor.Plugins.IO
{
    public sealed class ReadFiles : IPlugin
    {
        readonly IEnumerable<string> _files;
        readonly string _templateFileName;

        public ReadFiles(string path, string templateFileName)
            : this(path, "*", templateFileName)
        {
        }

        public ReadFiles(string path, string searchPattern, string templateFileName)
        {
            _files = Directory.EnumerateFiles(path, searchPattern);
            _templateFileName = templateFileName;
        }

        public async IAsyncEnumerable<IDocument> ExecuteAsync(IAsyncEnumerable<IDocument> inputs)
        {
            await foreach (var input in inputs.ConfigureAwait(false))
            {
                yield return input;
            }

            foreach (var file in _files)
            {
                var document = DocumentFactory.GetDocument(_templateFileName);
                document.Content.Add(nameof(ReadFiles), File.ReadAllBytes(file));

                yield return document;
            }
        }
    }
}