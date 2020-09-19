using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Stazor.Core;

namespace Stazor.Plugins.IO
{
    public sealed class ReadFiles : IPlugin
    {
        public static readonly byte[] Key = new byte[]
        {
            0x52, 0x65, 0x61, 0x64, 0x46, 0x69, 0x6C, 0x65, 0x73
        };

        readonly IEnumerable<string> _files;
        readonly string _templatePath;

        public ReadFiles(string path, string templateFileName)
            : this(path, "*", templateFileName)
        {
        }

        public ReadFiles(string path, string searchPattern, string templatePath)
        {
            _files = Directory.EnumerateFiles(path, searchPattern);
            _templatePath = templatePath;
        }

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