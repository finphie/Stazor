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

namespace Stazor.Plugins.IO
{
    /// <summary>
    /// Parses markdown and renders it to HTML.
    /// </summary>
    public sealed class ReadMarkdownFiles : IPlugin
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
        readonly ReadMarkdownFilesSettings _settings;

        readonly IEnumerable<string> _files;
        readonly HtmlRenderer _renderer;
        readonly StringWriter _writer;
        readonly IDeserializer _yamlDeserializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadMarkdownFiles"/> class.
        /// </summary>
        public ReadMarkdownFiles(IStazorLogger logger, ReadMarkdownFilesSettings settings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            _files = Directory.EnumerateFiles(_settings.Path, "*.md");

            // Markdown関連の設定
            _writer = new();
            _renderer = new(_writer);
            Pipeline.Setup(_renderer);

            // YAML関連の設定
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
                yield return input;
            }

            foreach (var file in _files)
            {
                // ファイル読み込み
                var data = File.ReadAllText(file);

                // Markdown
                var markdown = Markdig.Markdown.Parse(data, Pipeline);

                // YAML
                var title = markdown.Descendants<HeadingBlock>()
                    .First(x => x.Level == 1)
                    .Inline
                    .Descendants<LiteralInline>()
                    .First()
                    .Content;
                var yaml = markdown.Descendants<YamlFrontMatterBlock>().FirstOrDefault();

                // ドキュメント作成
                var document = DocumentFactory.GetDocument(_settings.TemplateFilePath);

                if (yaml is not null)
                {
                    var yamlString = data.Substring(yaml.Span.Start + 4, yaml.Span.Length - 8);
                    var metadata = _yamlDeserializer.Deserialize<Metadata>(yamlString);

                    document.Metadata.Title = metadata.Title;
                    document.Metadata.PublishedDate = metadata.PublishedDate;
                    document.Metadata.ModifiedDate = metadata.ModifiedDate > metadata.PublishedDate
                        ? metadata.ModifiedDate
                        : metadata.PublishedDate;
                    document.Metadata.Category = metadata.Category;
                    document.Metadata.Tags = metadata.Tags;
                }

                document.Metadata.Title = title.ToString();

                // MarkdownからHTMLに変換
                _renderer.Render(markdown);
                _writer.Flush();

                document.Content.Add(Key, Encoding.UTF8.GetBytes(_writer.ToString()));
                _writer.GetStringBuilder().Clear();

                yield return document;
            }

            _logger.Information("End");
        }
    }
}