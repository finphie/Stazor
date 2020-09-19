using System;
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
using Stazor.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Stazor.Plugins.Renderer
{
    public sealed class Markdown : IPlugin
    {
        public static readonly byte[] Key = new byte[]
        {
            0x4D, 0x61, 0x72, 0x6B, 0x64, 0x6F, 0x77, 0x6E
        };

        static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
            .UseAutoLinks()
            .UsePipeTables()
            .UseYamlFrontMatter()
            .Build();

        readonly byte[] _inputKey;
        readonly HtmlRenderer _renderer;
        readonly StringWriter _writer;

        public Markdown(byte[] inputKey)
        {
            _inputKey = inputKey;
            _writer = new();
            _renderer = new(_writer);
            Pipeline.Setup(_renderer);
        }

        public async IAsyncEnumerable<IDocument> ExecuteAsync(IAsyncEnumerable<IDocument> inputs)
        {
            await foreach (var input in inputs.ConfigureAwait(false))
            {
                if (!input.Content.TryGetValue(_inputKey, out var value))
                {
                    throw new ArgumentException(nameof(_inputKey));
                }

                var data = Encoding.UTF8.GetString(value);
                var markdown = Markdig.Markdown.Parse(data, Pipeline);
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

                    var yamlString = data.Substring(yaml.Span.Start + 4, yaml.Span.Length - 8);
                    input.Metadata = deserializer.Deserialize<Metadata>(yamlString);
                }

                input.Metadata.Title = title.ToString();

                _renderer.Render(markdown);
                _writer.Flush();

                input.Content.Add(Key, Encoding.UTF8.GetBytes(_writer.ToString()));
                //input.Content.Add(nameof(Markdown), _writer.ToString());
                _writer.GetStringBuilder().Clear();

                yield return input;
            }
        }
    }
}