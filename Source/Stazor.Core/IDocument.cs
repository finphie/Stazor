using System.Collections.Generic;

namespace Stazor.Core
{
    public interface IDocument
    {
        Dictionary<string, object> Content { get; }

        string TemplateFileName { get; init; }

        IMetadata Metadata { get; set; }
    }
}