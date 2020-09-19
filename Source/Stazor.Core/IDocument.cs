using System.Collections.Generic;

namespace Stazor.Core
{
    public interface IDocument
    {
        Dictionary<string, byte[]> Content { get; }

        string TemplatePath { get; init; }

        IMetadata Metadata { get; set; }
    }
}