using System;
using System.Buffers;
using System.Collections.Generic;
using Cysharp.Text;

namespace Stazor.Core.Plugins.Metadata
{
    public sealed class Viewport : IPlugin
    {
        public static readonly Viewport Default = new();

        readonly byte[] _html;

        public Viewport()
            : this("width=device-width, initial-scale=1")
        {

        }

        public Viewport(ReadOnlySpan<char> content)
        {
            using var builder = ZString.CreateUtf8StringBuilder(true);
            builder.Append("<meta name=\"viewport\" content=\"");
            builder.Append(content);
            builder.Append("\">");

            _html = new byte[builder.Length];
            builder.TryCopyTo(_html, out _);
        }

        public async IAsyncEnumerable<IDocument> ExecuteAsync(IAsyncEnumerable<IDocument> inputs)
        {
            await foreach (var input in inputs)
            {
                input.Content.Head.Write(_html);
                yield return input;
            }
        }
    }
}