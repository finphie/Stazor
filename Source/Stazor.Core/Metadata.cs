using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Stazor.Core
{
    [SuppressMessage("Naming", "CA1724:型名は名前空間と同一にすることはできません")]
    public sealed class Metadata : IMetadata
    {
        public string? Title { get; set; }

        public DateTimeOffset PublishedDate { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public string? Category { get; set; }

        public List<string>? Tags { get; }
    }
}