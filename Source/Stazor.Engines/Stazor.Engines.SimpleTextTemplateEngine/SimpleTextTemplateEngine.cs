using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SimpleTextTemplate;

namespace Stazor.Engines
{
    /// <summary>
    /// A simple template engine.
    /// </summary>
    public sealed class SimpleTextTemplateEngine : IEngine
    {
        readonly Dictionary<string, Template> _table = new();

        /// <inheritdoc/>
        public string Name => nameof(SimpleTextTemplateEngine);

        /// <inheritdoc/>
        public string Description => "SimpleTextTemplateライブラリを利用したエンジンです。";

        /// <inheritdoc/>
        public ValueTask ExecuteAsync(IBufferWriter<byte> bufferWriter, IStazorDocument document)
        {
            if (document is null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var path = document.TemplatePath;

            if (_table.TryGetValue(path, out var value))
            {
                value.RenderTo(bufferWriter, document.Context);
                return ValueTask.CompletedTask;
            }

            var file = File.ReadAllBytes(document.TemplatePath);
            var template = Template.Parse(file);
            _table.Add(path, template);

            template.RenderTo(bufferWriter, document.Context);

            return ValueTask.CompletedTask;
        }
    }
}