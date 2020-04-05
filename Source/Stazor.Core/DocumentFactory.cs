namespace Stazor.Core
{
    public static class DocumentFactory
    {
        public static IDocument GetDocument(string content)
            => new Document(content);
    }
}