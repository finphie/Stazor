using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Text;
using Stazor.Core;

namespace Stazor.Plugins.Metadata
{
    /// <summary>
    /// Set the CSS.
    /// </summary>
    public sealed class StyleSheet : IPlugin
    {
        /// <summary>
        /// The content key.
        /// </summary>
        public static readonly byte[] Key = new byte[]
        {
            0x53, 0x74, 0x79, 0x6C, 0x65, 0x53, 0x68, 0x65, 0x65, 0x74
        };

        readonly byte[] _html;

        /// <summary>
        /// Initializes a new instance of the <see cref="StyleSheet"/> class.
        /// </summary>
        /// <param name="href">The CSS file url.</param>
        public StyleSheet(ReadOnlySpan<char> href)
        {
            using var builder = ZString.CreateUtf8StringBuilder(true);
            builder.Append("<link rel=\"stylesheet preload\" as=\"style\" href=\"");
            builder.Append(href);
            builder.Append("\">");

            _html = new byte[builder.Length];
            builder.TryCopyTo(_html, out _);
        }

        /// <inheritdoc/>
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