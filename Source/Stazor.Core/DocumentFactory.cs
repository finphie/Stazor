namespace Stazor.Core
{
    public static class DocumentFactory
    {
        public static IDocument GetDocument(string templatePath)
            => new Document() { TemplatePath = templatePath };
    }
}