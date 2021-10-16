using System.ComponentModel.DataAnnotations;

namespace Stazor.Plugins.IO;

/// <summary>
/// <see cref="ReadMarkdownFiles"/>プラグインの設定クラスです。
/// </summary>
public sealed record ReadMarkdownFilesSettings
{
    /// <summary>
    /// キー
    /// </summary>
    public const string Key = nameof(ReadMarkdownFiles);

    /// <summary>
    /// コンテキストのキーを取得または設定します。
    /// </summary>
    /// <value>
    /// コンテキストのキー
    /// </value>
    [Required]
    public string ContextKey { get; init; } = "Markdown";

    /// <summary>
    /// Markdownが存在するディレクトリのパスを取得または設定します。
    /// </summary>
    /// <value>
    /// ディレクトリのパス
    /// </value>
    [Required]
#pragma warning disable CS8618
    public string Path { get; init; }
#pragma warning restore CS8618

    /// <summary>
    /// HTMLテンプレートファイルのパスを取得または設定します。
    /// </summary>
    /// <value>
    /// テンプレートファイルのパス
    /// </value>
    [Required]
    [FileExtensions(Extensions = ".html")]
#pragma warning disable CS8618
    public string TemplateFilePath { get; init; }
#pragma warning restore CS8618
}