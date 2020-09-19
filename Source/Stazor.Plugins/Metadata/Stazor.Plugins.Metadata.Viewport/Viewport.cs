using System;
using System.Collections.Generic;
using Cysharp.Text;
using Stazor.Core;

namespace Stazor.Plugins.Metadata
{
    public sealed class Viewport : IPlugin
    {
        public static readonly Viewport Default = new();

        public static readonly byte[] Key = new byte[]
        {
            0x56, 0x69, 0x65, 0x77, 0x70, 0x6F, 0x72, 0x74
        };

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
                input.Content.Add(Key, _html);

                yield return input;
            }
        }
    }
}