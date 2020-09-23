using System.Collections.Generic;

namespace Stazor.Core
{
    /// <summary>
    /// A document consists of a set of content and metadata attributes.
    /// </summary>
    public sealed class Document : IDocument
    {
        /// <inheritdoc/>
        public Dictionary<byte[], byte[]> Content { get; } = new(ByteArrayComparer.Default);

        /// <inheritdoc/>
        public string TemplatePath { get; init; }

        /// <inheritdoc/>
        public IMetadata Metadata { get; set; } = new Metadata();
    }
}