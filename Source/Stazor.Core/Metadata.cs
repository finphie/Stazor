using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Stazor.Core
{
    /// <summary>
    /// The document metadata.
    /// </summary>
    [SuppressMessage("Naming", "CA1724:型名は名前空間と同一にすることはできません", Justification = "Content metadata")]
    public sealed class Metadata : IMetadata
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