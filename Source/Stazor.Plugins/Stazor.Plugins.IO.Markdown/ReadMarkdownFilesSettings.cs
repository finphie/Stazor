using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Stazor.Plugins.IO
{
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
        public string ContextKey { get; set; }

        /// <summary>
        /// Markdownが存在するディレクトリのパスを取得、設定します。
        /// </summary>
        [Required]
        public string Path { get; set; }

        /// <summary>
        /// HTMLテンプレートファイルのパスを取得、設定します。
        /// </summary>
        [Required]
        [FileExtensions(Extensions = ".html")]
        [DisallowNull]
        public string? TemplateFilePath { get; init; }
    }
}