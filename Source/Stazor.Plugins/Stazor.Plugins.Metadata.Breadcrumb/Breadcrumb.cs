using Cysharp.Text;
using Stazor.Logging;
using Utf8Utility;

namespace Stazor.Plugins.Metadata;

/// <summary>
/// パンくずリストを作成します。
/// </summary>
public sealed class Breadcrumb : IEditDocumentPlugin
{
    readonly IStazorLogger _logger;
    readonly BreadcrumbSettings _settings;
    readonly Utf8Array _contextKey;
    readonly Utf8Array _jsonLdKey;

    /// <summary>
    /// <see cref="Breadcrumb"/>クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="logger">ロガー</param>
    /// <param name="settings">設定</param>
    public Breadcrumb(IStazorLogger<Breadcrumb> logger, BreadcrumbSettings settings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _contextKey = new(_settings.ContextKey);
        _jsonLdKey = new(_settings.JsonLdKey);
    }

    /// <inheritdoc/>
    public void Execute(IStazorDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);
        _logger.Debug("Start");

        using var builder = ZString.CreateUtf8StringBuilder();

        // TODO: UTF-8にする。
        // TODO: "ホーム"文字列を可変
        builder.Append("<nav><ol class=\"breadcrumbs\"><li><a href=\"/\">ホーム</a><li><a href=\"");
        builder.Append(document.Metadata.Category);
        builder.Append("\">");
        builder.Append(document.Metadata.Category);
        builder.Append("</a>");
        builder.Append("<li>");
        builder.Append(document.Metadata.Title);
        builder.Append("</ol>");
        builder.Append("</nav>");

        document.Context.Add(_contextKey, new Utf8Array(builder.AsSpan().ToArray()));

        if (_settings.JsonLd)
        {
            // TODO: UTF-8にする。
            const string Json1 = "{\"@context\":\"https://schema.org\",\"@type\":\"BreadcrumbList\",\"itemListElement\":" +
                "[{\"@type\":\"ListItem\",\"position\":0,\"name\":\"ホーム\",\"item\":\"/\"}," +
                "{\"@type\":\"ListItem\",\"position\":1,\"name\":\"";
            const string Json2 = "\",\"item\":\"/";
            const string Json3 = "\"}]}";

            builder.Clear();
            builder.Append(Json1);
            builder.Append(document.Metadata.Category);
            builder.Append(Json2);
            builder.Append(document.Metadata.Category);
            builder.Append(Json3);

            document.Context.Add(_jsonLdKey, new Utf8Array(builder.AsSpan().ToArray()));
        }

        _logger.Debug("End");
    }
}