using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Stazor.Core;
using Stazor.Engine;

namespace Stazor.Engines.Simple
{
    /// <summary>
    /// A simple template engine.
    /// </summary>
    public sealed class SimpleEngine : IEngine
    {
        /// <summary>
        /// Gets a singleton instance of the <see cref="SimpleEngine"/>.
        /// </summary>
        public static readonly SimpleEngine Default = new();

        readonly Dictionary<string, TemplateCache> _table = new();

        SimpleEngine()
        {
        }

        /// <inheritdoc/>
        public string Name => "Simple";

        /// <inheritdoc/>
        public string Description => "Simple templates";

#pragma warning disable CS1998 // 非同期メソッドは、'await' 演算子がないため、同期的に実行されます
        /// <inheritdoc/>
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