using System;
using System.Buffers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Text;
using Stazor.Core;

namespace Stazor.Plugins.Metadata
{
    public sealed class StyleSheet : IPlugin
    {
        readonly byte[] _html;

        public StyleSheet(ReadOnlySpan<char> href)
        {
            using var builder = ZString.CreateUtf8StringBuilder(true);
            builder.Append("<link rel=\"stylesheet preload\" as=\"style\" href=\"");
            builder.Append(href);
            builder.Append("\">");

            _html = new byte[builder.Length];
            builder.TryCopyTo(_html, out _);
        }

        public async IAsyncEnumerable<IDocument> ExecuteAsync(IAsyncEnumerable<IDocument> inputs)
        {
            await foreach (var input in inputs.ConfigureAwait(false))
            {
                input.Content.Head.Write(_html);
                yield return input;
            }
        }
    }
}