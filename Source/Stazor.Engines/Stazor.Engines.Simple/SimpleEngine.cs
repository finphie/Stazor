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
        public static readonly SimpleEngine Default = new();

        public string Name => "Simple";

        public string Description => "Simple templates";

        readonly Dictionary<string, TemplateCache> _table = new();

        SimpleEngine()
        {
        }

        public async ValueTask ExecuteAsync(IBufferWriter<byte> bufferWriter, IDocument document)
        {
            var path = document.TemplatePath;

            if (_table.TryGetValue(path, out var value))
            {
                value.RenderTo(bufferWriter, document.Content);
                return;
            }

            var file = File.ReadAllBytes(document.TemplatePath);
            var cache = new TemplateCache(file);
            _table.Add(path, cache);

            cache.RenderTo(bufferWriter, document.Content);

            // Console.WriteLine(Encoding.UTF8.GetString((document.Content["Markdown"] as byte[])!));
        }   
    }
}