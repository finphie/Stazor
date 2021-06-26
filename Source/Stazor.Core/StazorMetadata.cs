using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Stazor.Core
{
    /// <summary>
    /// The document metadata.
    /// </summary>
    public sealed class StazorMetadata : IMetadata
    {
        /// <inheritdoc/>
        [AllowNull]
        public string Title { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset PublishedDate { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset ModifiedDate { get; set; }

        /// <inheritdoc/>
        [AllowNull]
        public string Category { get; set; }

        /// <inheritdoc/>
        [AllowNull]
        public IReadOnlySet<string> Tags { get; set; }
    }
}