﻿using System.Diagnostics.CodeAnalysis;
using System.IO;
using Stazor.Core;
using Stazor.Plugins.IO.Helpers;
using Utf8Utility;

namespace Stazor.Plugins.IO
{
    public sealed record ReadMarkdownFilesSettings : IStazorKey, IValidatable
    {
        /// <inheritdoc/>
        public Utf8String Key { get; init; }

        /// <summary>
        /// Markdownが存在するフォルダのパスを取得、設定します。
        /// </summary>
        public string Path { get; init; }

        /// <summary>
        /// HTMLテンプレートファイルのパスを取得、設定します。
        /// </summary>
        [DisallowNull]
        public string? TemplateFilePath { get; init; }

        /// <inheritdoc/>
        public void Validate()
        {
            if (Key == Utf8String.Empty)
            {
                ThrowHelper.ThrowArgumentNullException(nameof(Key));
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