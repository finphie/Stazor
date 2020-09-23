using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Text;
using Stazor.Core;

namespace Stazor.Plugins.Metadata
{
    /// <summary>
    /// Set the viewport.
    /// </summary>
    public sealed class Viewport : IPlugin
    {
        /// <summary>
        /// Gets a singleton instance of the <see cref="Viewport"/>.
        /// </summary>
        public static readonly Viewport Default = new();

        /// <summary>
        /// The content key.
        /// </summary>
        public static readonly byte[] Key = new byte[]
        {
            0x56, 0x69, 0x65, 0x77, 0x70, 0x6F, 0x72, 0x74
        };

        readonly byte[] _html;

        /// <summary>
        /// Initializes a new instance of the <see cref="Viewport"/> class.
        /// </summary>
        public Viewport()
            : this("width=device-width, initial-scale=1")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Viewport"/> class.
        /// </summary>
        /// <param name="content">The "meta" content attribute.</param>
        public Viewport(ReadOnlySpan<char> content)
        {
            using var builder = ZString.CreateUtf8StringBuilder(true);
            builder.Append("<meta name=\"viewport\" content=\"");
            builder.Append(content);
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