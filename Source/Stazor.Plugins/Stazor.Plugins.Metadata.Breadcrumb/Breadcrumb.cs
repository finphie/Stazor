using System.Runtime.Serialization;
using Cysharp.Text;
using Stazor.Logging;
using Utf8Json;
using Utf8Json.Resolvers;
using Utf8Utility;

namespace Stazor.Plugins.Metadata;

/// <summary>
/// パンくずリストを作成します。
/// </summary>
public sealed class Breadcrumb : IEditDocumentPlugin
{
    readonly IStazorLogger _logger;
    readonly BreadcrumbSettings _settings;
    readonly Utf8String _contextKey;
    readonly Utf8String _jsonLdKey;

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

        using var builder = ZString.CreateUtf8StringBuilder(true);

        // TODO: "ホーム"文字列を可変
        builder.Append("<nav><ol class=\"breadcrumbs\"><li><a href=\"/\">ホーム</a><li><a href=\"");

        var length = builder.Length;

        builder.Append(document.Metadata.Category);
        builder.Append("\">");
        builder.Append(document.Metadata.Category);
        builder.Append("</a>");
        builder.Append("<li>");
        builder.Append(document.Metadata.Title);

        builder.Append("</ol>");
        builder.Append("</nav>");

        document.Context.Add(_contextKey, new Utf8String(builder.AsSpan().ToArray()));

        if (_settings.JsonLd)
        {
            builder.Advance(-length);

            // TODO: JSON-LD 高速化
            var json = new JsonLd
            {
                Context = "https://schema.org",
                Type = "BreadcrumbList",
                Items = new JsonLd.ItemListElement[2]
            };

            for (var i = 0; i < json.Items.Length; i++)
            {
                json.Items[i] = new();
                json.Items[i].Type = "ListItem";
                json.Items[i].Position = i + 1;
            }

            json.Items[0].Name = "ホーム";
            json.Items[1].Name = document.Metadata.Category!;
            json.Items[0].Item = "/";
            json.Items[1].Item = "/" + document.Metadata.Category;

            var jsonLd = JsonSerializer.Serialize(json, StandardResolver.AllowPrivateExcludeNullSnakeCase);
            document.Context.Add(_jsonLdKey, new Utf8String(jsonLd));
        }

        _logger.Debug("End");
    }

    sealed class JsonLd
    {
        [DataMember(Name = "@context")]
        public string? Context { get; set; }

        [DataMember(Name = "@type")]
        public string? Type { get; set; }

        [DataMember(Name = "itemListElement")]
        public ItemListElement[]? Items { get; set; }

        public sealed class ItemListElement
        {
            [DataMember(Name = "@type")]
            public string? Type { get; set; }

            public int Position { get; set; }

            public string? Name { get; set; }

            public string? Item { get; set; }
        }
    }
}