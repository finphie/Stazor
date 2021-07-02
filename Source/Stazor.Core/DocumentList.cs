using System.Collections.Generic;

namespace Stazor.Core
{
    public sealed class DocumentList : IDocumentList
    {
        public int Length => _documents.Count;

        public IStazorDocument this[int index]
        {
            get => _documents[index];
            set => _documents[index] = value;
        }

        readonly List<IStazorDocument> _documents = new();

        public void Add(IStazorDocument document)
        {
            _documents.Add(document);
        }
    }
}