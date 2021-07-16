using System.Diagnostics.CodeAnalysis;
using System.IO;
using Stazor.Plugins.IO.Helpers;

namespace Stazor.Plugins.IO
{
    public sealed record ReadMarkdownFilesSettings : IPluginSettingsKey
    {
        public static string Key => "Markdown";

        /// <summary>
        /// Markdownが存在するフォルダのパスを取得、設定します。
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// HTMLテンプレートファイルのパスを取得、設定します。
        /// </summary>
        [DisallowNull]
        public string? TemplateFilePath { get; init; }

        /// <inheritdoc/>
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