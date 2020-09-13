namespace Stazor.Core
{
    public static class DocumentFactory
    {
        public static IDocument GetDocument(string templateFileName)
            => new Document() { TemplateFileName = templateFileName };
    }
}