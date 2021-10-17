using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Stazor.Core;
using Stazor.Logging;
using Stazor.Plugins.IO.Helpers;
using Utf8Utility;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Stazor.Plugins.IO;

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
    readonly Utf8Array _contextKey;

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
        ThrowHelper.ThrowFileNotFoundExceptionIfFileNotFound(filePath);
        _logger.Debug("Start");

        var data = File.ReadAllText(filePath);
        var markdown = Markdown.Parse(data, Pipeline);
        var yaml = markdown.Descendants<YamlFrontMatterBlock>().FirstOrDefault();

        // TODO: 例外メッセージ
        if (yaml is null)
        {
            throw new InvalidOperationException();
        }

        // H1タグになる部分をタイトルとする。
        var title = markdown.Descendants<HeadingBlock>()
            ?.First(x => x.Level == 1)
            ?.Inline
            ?.Descendants<LiteralInline>()
            .First()
            .Content
            .ToString();

        if (title is null)
        {
            throw new InvalidOperationException();
        }

        var yamlString = data.Substring(yaml.Span.Start + 4, yaml.Span.Length - 8);
        var metadata2 = _yamlDeserializer.Deserialize<StazorMetadata>(yamlString);
        var modifiedDate = metadata2.ModifiedDate > metadata2.PublishedDate
            ? metadata2.ModifiedDate
            : metadata2.PublishedDate;

        var metadata = Metadata.Create(title, metadata2.PublishedDate, modifiedDate, metadata2.Category, metadata2.Tags);

        // MarkdownからHTMLに変換
        var writer = new StringWriter();
        var renderer = new HtmlRenderer(writer);
        Pipeline.Setup(renderer);
        renderer.Render(markdown);
        writer.Flush();

        var document = Document.Create(_settings.TemplateFilePath, metadata);
        document.Context.Add(_contextKey, (Utf8Array)writer.ToString());

        _logger.Debug("End");

        return document;
    }
}