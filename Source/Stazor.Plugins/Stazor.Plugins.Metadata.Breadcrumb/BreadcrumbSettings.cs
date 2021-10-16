using System.ComponentModel.DataAnnotations;

namespace Stazor.Plugins.Metadata;

/// <summary>
/// <see cref="Breadcrumb"/>プラグインの設定クラスです。
/// </summary>
public sealed record BreadcrumbSettings
{
    /// <summary>
    /// キー
    /// </summary>
    public const string Key = nameof(Breadcrumb);

    /// <summary>
    /// コンテキストのキーを取得または設定します。
    /// </summary>
    /// <value>
    /// コンテキストのキー
    /// </value>
    [Required]
    public string ContextKey { get; init; } = "Breadcrumb";

    /// <summary>
    /// JSON-LDのキーを取得または設定します。
    /// </summary>
    /// <value>
    /// JSON-LDのキー
    /// </value>
    [Required]
    public string JsonLdKey { get; init; } = "JsonLd";

    /// <summary>
    /// JSON-LDの出力可否を取得または設定します。
    /// </summary>
    /// <value>
    /// JSON-LDの出力可否
    /// </value>
    public bool JsonLd { get; set; }
}