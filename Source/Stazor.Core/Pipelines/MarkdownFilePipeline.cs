using System.Collections.Generic;
using Stazor.Core.Plugins;

namespace Stazor.Core.Pipelines
{
    public sealed class MarkdownFilePipeline : Pipeline
    {
        public MarkdownFilePipeline(string path)
            : base(GetPlugin(path))
        {
        }

        static IEnumerable<IPlugin> GetPlugin(string path)
            => new IPlugin[]
            {
                new ReadFiles(path, "*.md"),
                new Markdown()
            };
    }
}