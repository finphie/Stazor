using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SimpleTextTemplate;
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

        readonly Dictionary<string, Template> _table = new();

        SimpleEngine()
        {
        }

        /// <inheritdoc/>
        public string Name => "Simple";

        /// <inheritdoc/>
        public string Description => "Simple templates";

        /// <inheritdoc/>
        public ValueTask ExecuteAsync(IBufferWriter<byte> bufferWriter, IDocument document)
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