using System;

namespace Stazor.Core
{
    public sealed class Document : IDocument
    {
        public HtmlContent Content { get; } = new();

        public IMetadata Metadata { get; set; } = new Metadata();

        public Document(byte[] content)
        {
            var span = Content.Body.Main.Article.GetSpan(content.Length);
            content.AsSpan().CopyTo(span);
            Content.Body.Main.Article.Advance(content.Length);
        }
    }
}