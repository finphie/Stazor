using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Stazor.Plugins.Metadata;

/// <summary>
/// <see cref="Breadcrumb"/>プラグインの設定クラスです。
/// </summary>
public sealed record BreadcrumbSettings : ISettingsKey
{
    /// <summary>
    /// キーを取得または設定します。
    /// </summary>
    /// <value>
    /// キー
    /// </value>
    public static string Key => nameof(Breadcrumb);

    /// <summary>
    /// コンテキストのキーを取得または設定します。
    /// </summary>
    /// <value>
    /// コンテキストのキー
    /// </value>
    [Required]
    [AllowNull]
    public string ContextKey { get; init; } = "Breadcrumb";

    /// <summary>
    /// JSON-LDのキーを取得または設定します。
    /// </summary>
    /// <value>
    /// JSON-LDのキー
    /// </value>
    [Required]
    [AllowNull]
    public string JsonLdKey { get; init; } = "JsonLd";

    /// <summary>
    /// JSON-LDの出力可否を取得または設定します。
    /// </summary>
    /// <value>
    /// JSON-LDの出力可否
    /// </value>
    public bool JsonLd { get; set; }
}