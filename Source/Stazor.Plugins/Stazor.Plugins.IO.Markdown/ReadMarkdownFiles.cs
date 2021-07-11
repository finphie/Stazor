using System;
using System.IO;
using System.Linq;
using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Stazor.Core;
using Stazor.Logging;
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

        readonly IDeserializer _yamlDeserializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadMarkdownFiles"/> class.
        /// </summary>
        public ReadMarkdownFiles(IStazorLogger<ReadMarkdownFiles> logger, ReadMarkdownFilesSettings settings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            // Markdown関連の設定
            //_writer = new();
            //_renderer = new(_writer);
            //Pipeline.Setup(_renderer);

            // YAML関連の設定
            _yamlDeserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .WithNodeTypeResolver(SortedSetResolver.Default)
                .Build();
        }

        /// <inheritdoc/>
        public IStazorDocument CreateDocument(string filePath)
        {
            _logger.Debug("Start");

            //var pipeline = new MarkdownPipelineBuilder()
            //.UseAutoLinks()
            //.UsePipeTables()
            //.UseYamlFrontMatter()
            //.Build();

            // ファイル読み込み
            var data = File.ReadAllText(filePath);

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
            var writer = new StringWriter();
            var renderer = new HtmlRenderer(writer);
            Pipeline.Setup(renderer);
            renderer.Render(markdown);
            writer.Flush();

            document.Context.Add(_settings.Key, (Utf8String)writer.ToString());
            //writer.GetStringBuilder().Clear();

            _logger.Debug("End");

            return document;
        }
    }
}