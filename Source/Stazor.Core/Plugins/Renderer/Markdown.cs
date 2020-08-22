using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Stazor.Core.Plugins.Renderer
{
    public sealed class Markdown : IPlugin
    {
        public static readonly Markdown Default;

        static readonly MarkdownPipeline Pipeline;

        readonly HtmlRenderer _renderer;
        readonly StringWriter _writer;

        static Markdown()
        {
            Pipeline = new MarkdownPipelineBuilder()
                .UseAutoLinks()
                .UsePipeTables()
                .UseYamlFrontMatter()
                .Build();

            Default = new();
        }

        Markdown()
        {
            _writer = new();
            _renderer = new(_writer);
            Pipeline.Setup(_renderer);
        }

        public async IAsyncEnumerable<IDocument> ExecuteAsync(IAsyncEnumerable<IDocument> inputs)
        {
            await foreach (var input in inputs.ConfigureAwait(false))
            {
                var body = Encoding.UTF8.GetString(input.Content.Body.Main.Article.WrittenSpan);

                var markdown = Markdig.Markdown.Parse(body, Pipeline);
                var title = markdown.Descendants<HeadingBlock>()
                    .First(x => x.Level == 1)
                    .Inline
                    .Descendants<LiteralInline>()
                    .First()
                    .Content;
                var yaml = markdown.Descendants<YamlFrontMatterBlock>().FirstOrDefault();

                if (yaml is not null)
                {
                    var deserializer = new DeserializerBuilder()
                        .WithNamingConvention(UnderscoredNamingConvention.Instance)
                        .Build();

                    var yamlString = body.Substring(yaml.Span.Start + 4, yaml.Span.Length - 8);
                    input.Metadata = deserializer.Deserialize<Core.Metadata>(yamlString);
                }

                input.Metadata.Title = title.ToString();

                _renderer.Render(markdown);
                _writer.Flush();

                input.Content.Body.Main.Article.Clear();
                Encoding.UTF8.GetBytes(_writer.ToString(), input.Content.Body.Main.Article);
                _writer.GetStringBuilder().Clear();

                yield return input;
            }
        }
    }
}