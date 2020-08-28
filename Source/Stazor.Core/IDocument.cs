using System.Collections.Generic;

namespace Stazor.Core
{
    public interface IDocument
    {
        HtmlContent Content { get; }

        IMetadata Metadata { get; set; }
    }
}