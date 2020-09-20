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

#pragma warning disable CS1998 // 非同期メソッドは、'await' 演算子がないため、同期的に実行されます
        public async ValueTask ExecuteAsync(IBufferWriter<byte> bufferWriter, IDocument document)
#pragma warning restore CS1998 // 非同期メソッドは、'await' 演算子がないため、同期的に実行されます
        {
            if (document is null)
            {
                throw new ArgumentNullException(nameof(document));
            }

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
        }   
    }
}