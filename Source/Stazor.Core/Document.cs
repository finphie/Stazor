using System.Collections.Generic;

namespace Stazor.Core
{
    public sealed class Document : IDocument
    {
        public Dictionary<byte[], byte[]> Content { get; } = new(ByteArrayComparer.Default);

        public string TemplatePath { get; init; }

        public IMetadata Metadata { get; set; } = new Metadata();
    }
}