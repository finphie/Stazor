using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Stazor.Plugins.IO;

/// <summary>
/// <see cref="ReadMarkdownFiles"/>プラグインの設定クラスです。
/// </summary>
public sealed record ReadMarkdownFilesSettings : ISettingsKey
{
    /// <summary>
    /// キーを取得または設定します。
    /// </summary>
    /// <value>
    /// キー
    /// </value>
    public static string Key => nameof(ReadMarkdownFiles);

    /// <summary>
    /// コンテキストのキーを取得または設定します。
    /// </summary>
    /// <value>
    /// コンテキストのキー
    /// </value>
    [Required]
    [AllowNull]
    public string ContextKey { get; init; } = "Markdown";

    /// <summary>
    /// Markdownが存在するディレクトリのパスを取得または設定します。
    /// </summary>
    /// <value>
    /// ディレクトリのパス
    /// </value>
    [Required]
    [AllowNull]
    public string Path { get; init; }

    /// <summary>
    /// HTMLテンプレートファイルのパスを取得または設定します。
    /// </summary>
    /// <value>
    /// テンプレートファイルのパス
    /// </value>
    [Required]
    [FileExtensions(Extensions = ".html")]
    [AllowNull]
    public string TemplateFilePath { get; init; }
}