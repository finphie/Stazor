namespace Stazor.Core
{
    public static class DocumentFactory
    {
        public static IDocument GetDocument(byte[] content)
            => new Document(content);
    }
}