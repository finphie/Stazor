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
    /// マークダウンファイルを解析してHTMLにレンダリングします。
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
        readonly Utf8String _contextKey;

        readonly IDeserializer _yamlDeserializer;

        /// <summary>
        /// <see cref="ReadMarkdownFiles"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="logger">ロガー</param>
        /// <param name="settings">設定</param>
        public ReadMarkdownFiles(IStazorLogger<ReadMarkdownFiles> logger, ReadMarkdownFilesSettings settings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _contextKey = new(_settings.ContextKey);

            _yamlDeserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .WithNodeTypeResolver(SortedSetResolver.Default)
                .Build();
        }

        /// <inheritdoc/>
        public IStazorDocument CreateDocument(string filePath)
        {
            _logger.Debug("Start");

            // ファイル読み込み
            var data = File.ReadAllText(filePath);

            // Markdown
            var markdown = Markdown.Parse(data, Pipeline);

            // YAML
            var title = markdown.Descendants<HeadingBlock>()
                ?.First(x => x.Level == 1)
                ?.Inline
                ?.Descendants<LiteralInline>()
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

            document.Context.Add(_contextKey, (Utf8String)writer.ToString());

            _logger.Debug("End");

            return document;
        }
    }
}