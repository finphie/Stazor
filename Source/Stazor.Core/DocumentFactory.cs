namespace Stazor.Core
{
    public static class DocumentFactory
    {
        public static IDocument GetDocument()
            => new Document();
    }
}