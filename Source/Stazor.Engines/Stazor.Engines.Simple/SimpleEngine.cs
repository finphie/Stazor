using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Stazor.Core;
using Stazor.Engine;

namespace Stazor.Engines.Simple
{
    public sealed class SimpleEngine : IEngine
    {
        public string Name => "Simple";

        public string Description => "Simple templates";

        readonly string _templatePath;
        readonly Dictionary<string, TemplateCache> _table;

        public SimpleEngine(string templatePath)
        {
            _templatePath = templatePath;
            _table = new();
        }

        public async ValueTask ExecuteAsync(IBufferWriter<byte> bufferWriter, IDocument document)
        {
            Console.WriteLine(document.TemplateFileName);

            if (_table.TryGetValue(document.TemplateFileName, out var value))
            {
                value.Debug();
                return;
            }

            var file = File.ReadAllBytes(Path.Combine(_templatePath, document.TemplateFileName));
            var c = new TemplateCache(file);
            _table.Add(document.TemplateFileName, c);

            c.Debug();

            // Console.WriteLine(Encoding.UTF8.GetString((document.Content["Markdown"] as byte[])!));
        }   
    }
}