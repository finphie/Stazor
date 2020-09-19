using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Text;
using Stazor.Core;

namespace Stazor.Plugins.Metadata
{
    public sealed class StyleSheet : IPlugin
    {
        public static readonly byte[] Key = new byte[]
        {
            0x53, 0x74, 0x79, 0x6C, 0x65, 0x53, 0x68, 0x65, 0x65, 0x74
        };

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
                input.Content.Add(Key, _html);
                yield return input;
            }
        }
    }
}