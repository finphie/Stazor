using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Stazor.Core;
using Utf8Utility;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Stazor.Plugins.IO
{
    /// <summary>
    /// Parses markdown and renders it to HTML.
    /// </summary>
    public sealed class ReadMarkdownFiles : INewDocumentsPlugin
    {
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
        public ValueTask<IDocumentList> CreateDocumentsAsync()
        {
            _logger.Information("Start");

            IDocumentList documents = new DocumentList();

            foreach (var file in _files)
            {
                // ファイル読み込み
                var data = File.ReadAllText(file);

                // Markdown
                var markdown = Markdown.Parse(data, Pipeline);

                // YAML
                var title = markdown.Descendants<HeadingBlock>()
                    .First(x => x.Level == 1)
                    .Inline
                    .Descendants<LiteralInline>()
                    .First()
                    .Content;
                var yaml = markdown.Descendants<YamlFrontMatterBlock>().FirstOrDefault();

                // ドキュメント作成
                var document = Document.Create(_settings.TemplateFilePath);

                if (yaml is not null)
                {
                    var yamlString = data.Substring(yaml.Span.Start + 4, yaml.Span.Length - 8);
                    var metadata = _yamlDeserializer.Deserialize<StazorMetadata>(yamlString);

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

                document.Context.Add(_settings.Key, (Utf8String)_writer.ToString());
                _writer.GetStringBuilder().Clear();

                documents.Add(document);
            }

            _logger.Information("End");

            return ValueTask.FromResult(documents);
        }
    }
}