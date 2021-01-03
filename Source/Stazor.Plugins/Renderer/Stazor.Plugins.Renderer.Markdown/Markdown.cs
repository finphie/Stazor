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
    /// <summary>
    /// Parses markdown and renders it to HTML.
    /// </summary>
    public sealed class Markdown : IPlugin
    {
        /// <summary>
        /// The content key.
        /// </summary>
        public static readonly byte[] Key = new byte[]
        {
            0x4D, 0x61, 0x72, 0x6B, 0x64, 0x6F, 0x77, 0x6E
        };

        static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
            .UseAutoLinks()
            .UsePipeTables()
            .UseYamlFrontMatter()
            .Build();

        readonly IStazorLogger _logger;
        readonly MarkdownSettings _settings;
        readonly byte[] _inputKey;
        readonly HtmlRenderer _renderer;
        readonly StringWriter _writer;
        readonly IDeserializer _yamlDeserializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Markdown"/> class.
        /// </summary>
        public Markdown(IStazorLogger logger, MarkdownSettings settings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            _inputKey = Encoding.UTF8.GetBytes(_settings.InputKey!);
            _writer = new();
            _renderer = new(_writer);
            Pipeline.Setup(_renderer);

            _yamlDeserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .WithNodeTypeResolver(SortedSetResolver.Default)
                .Build();
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<IDocument> ExecuteAsync(IAsyncEnumerable<IDocument> inputs)
        {
            _logger.Information("Start");

#pragma warning disable CA1508 // 使用されない条件付きコードを回避する
            await foreach (var input in inputs.ConfigureAwait(false))
#pragma warning restore CA1508 // 使用されない条件付きコードを回避する
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
                    var yamlString = data.Substring(yaml.Span.Start + 4, yaml.Span.Length - 8);
                    var metadata = _yamlDeserializer.Deserialize<Metadata>(yamlString);

                    input.Metadata.Title = metadata.Title;
                    input.Metadata.PublishedDate = metadata.PublishedDate;
                    input.Metadata.ModifiedDate = metadata.ModifiedDate > metadata.PublishedDate
                        ? metadata.ModifiedDate
                        : metadata.PublishedDate;
                    input.Metadata.Category = metadata.Category;
                    input.Metadata.Tags = metadata.Tags;
                }

                input.Metadata.Title = title.ToString();

                _renderer.Render(markdown);
                _writer.Flush();

                input.Content.Add(Key, Encoding.UTF8.GetBytes(_writer.ToString()));
                _writer.GetStringBuilder().Clear();

                yield return input;
            }

            _logger.Information("End");
        }
    }
}