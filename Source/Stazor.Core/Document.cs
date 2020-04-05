namespace Stazor.Core
{
    public class Document : IDocument
    {
        public string Content { get; }

        public Document(string content)
            => Content = content;
    }
}