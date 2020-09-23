using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Stazor.Core
{
    [SuppressMessage("Naming", "CA1724:型名は名前空間と同一にすることはできません", Justification = "Content metadata")]
    public sealed class Metadata : IMetadata
    {
        /// <inheritdoc/>
        public string? Title { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset PublishedDate { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset? ModifiedDate { get; set; }

        /// <inheritdoc/>
        public string? Category { get; set; }

        /// <inheritdoc/>
        public List<string>? Tags { get; }
    }
}