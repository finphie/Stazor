﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Text;
using Stazor.Core;

namespace Stazor.Plugins.Metadata
{
    /// <summary>
    /// Set the favicon.
    /// </summary>
    public sealed class Favicon : IPlugin
    {
        /// <summary>
        /// The content key.
        /// </summary>
        public static readonly byte[] Key = new byte[]
        {
            0x46, 0x61, 0x76, 0x69, 0x63, 0x6F, 0x6E
        };

        readonly IStazorLogger _logger;
        readonly FaviconSettings _settings;
        readonly byte[] _html;

        /// <summary>
        /// Initializes a new instance of the <see cref="Favicon"/> class.
        /// </summary>
        public Favicon(IStazorLogger logger, FaviconSettings settings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            using var builder = ZString.CreateUtf8StringBuilder(true);
            builder.Append("<link rel=\"icon\" href=\"");
            builder.Append(_settings.Href);
            builder.Append('\"');

            var extension = Path.GetExtension(_settings.Href.AsSpan());

            var type = extension.SequenceEqual(".ico") ? null
                : extension.SequenceEqual(".svg") ? "image/svg+xml"
                : extension.SequenceEqual(".png") ? "png"
                : (extension.SequenceEqual(".jpg") || extension.SequenceEqual(".jpeg")) ? "jpg"
                : throw new ArgumentOutOfRangeException(nameof(settings));

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