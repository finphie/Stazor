using System.Collections.Generic;

namespace Stazor.Core
{
    public sealed class DocumentList : IDocumentList
    {
        readonly List<IStazorDocument> _documents = new();

        public void Add(IStazorDocument document)
        {
            _documents.Add(document);
        }
    }
}