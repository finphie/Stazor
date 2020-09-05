using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Text;
using Stazor.Core;

namespace Stazor.Plugins.Metadata
{
    public sealed class Favicon : IPlugin
    {
        readonly byte[] _html;

        public Favicon(ReadOnlySpan<char> href)
        {
            using var builder = ZString.CreateUtf8StringBuilder(true);
            builder.Append("<link rel=\"icon\" href=\"");
            builder.Append(href);
            builder.Append('\"');

            var extension = Path.GetExtension(href);

            var type = extension.SequenceEqual(".ico") ? null
                : extension.SequenceEqual(".svg") ? "image/svg+xml"
                : extension.SequenceEqual(".png") ? "png"
                : (extension.SequenceEqual(".jpg") || extension.SequenceEqual(".jpeg")) ? "jpg"
                : throw new ArgumentException(nameof(href));

            if (type is not null)
            {
                builder.Append(" type=\"");
                builder.Append(type);
                builder.Append('\"');
            }

            builder.Append('>');

            _html = new byte[builder.Length];
            builder.TryCopyTo(_html, out _);
        }

        public async IAsyncEnumerable<IDocument> ExecuteAsync(IAsyncEnumerable<IDocument> inputs)
        {
            await foreach (var input in inputs.ConfigureAwait(false))
            {
                input.Content.Add(nameof(Favicon), _html);
                yield return input;
            }
        }
    }
}