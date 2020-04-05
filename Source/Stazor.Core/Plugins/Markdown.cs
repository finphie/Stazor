using System.Collections.Generic;
using MarkdownParser = Markdig.Markdown;

namespace Stazor.Core.Plugins
{
    public sealed class Markdown : IPlugin
    {
        public async IAsyncEnumerable<IDocument> ExecuteAsync(IAsyncEnumerable<IDocument> inputs)
        {
            await foreach (var input in inputs)
            {
                var document = DocumentFactory.GetDocument(MarkdownParser.ToHtml(input.Content));
                yield return document;
            }
        }
    }
}