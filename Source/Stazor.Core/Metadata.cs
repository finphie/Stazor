using System;
using System.Collections.Generic;

namespace Stazor.Core
{
    public class Metadata : IMetadata
    {
        public string? Title { get; set; }

        public DateTimeOffset Date { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public string? Category { get; set; }

        public List<string>? Tags { get; set; }
    }
}