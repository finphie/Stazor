using System.Collections.Generic;

namespace Stazor.Core
{
    public interface IDocument
    {
        Dictionary<string, object> Content { get; }

        IMetadata Metadata { get; set; }
    }
}