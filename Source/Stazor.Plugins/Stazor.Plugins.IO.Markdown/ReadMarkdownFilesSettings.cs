using System.Diagnostics.CodeAnalysis;
using System.IO;
using Stazor.Plugins.IO.Helpers;

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
        public string ContextKey { get; set; }

        /// <summary>
        /// Markdownが存在するディレクトリのパスを取得、設定します。
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// HTMLテンプレートファイルのパスを取得、設定します。
        /// </summary>
        [DisallowNull]
        public string? TemplateFilePath { get; init; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Key))
            {
                ThrowHelper.ThrowArgumentNullOrWhitespaceException(nameof(Key));
            }

            if (string.IsNullOrWhiteSpace(Path))
            {
                ThrowHelper.ThrowArgumentNullOrWhitespaceException(nameof(Path));
            }

            if (!Directory.Exists(Path))
            {
                ThrowHelper.ThrowDirectoryNotFoundException();
            }

            if (string.IsNullOrWhiteSpace(TemplateFilePath))
            {
                ThrowHelper.ThrowArgumentNullOrWhitespaceException(nameof(TemplateFilePath));
            }

            if (!File.Exists(TemplateFilePath))
            {
                ThrowHelper.ThrowFileNotFoundException(TemplateFilePath);
            }
        }
    }
}