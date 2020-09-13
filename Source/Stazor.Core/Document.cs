using System.Collections.Generic;

namespace Stazor.Core
{
    public sealed class Document : IDocument
    {
        public Dictionary<string, object> Content { get; } = new();

        public string TemplatePath { get; init; }

        public IMetadata Metadata { get; set; } = new Metadata();
    }
}