using System.Diagnostics.CodeAnalysis;
using System.IO;
using Stazor.Core;
using Stazor.Core.Helpers;

namespace Stazor.Plugins.IO
{
    public sealed class ReadFilesSettings : IStazorSettings, IValidatable
    {
        /// <summary>
        /// The relative or absolute path to the directory to search.
        /// </summary>
        public string Path { get; init; } = ".";

        // TODO: 変数名変更
        /// <summary>
        /// The relative or absolute path to the template directory.
        /// </summary>
        [DisallowNull]
        public string? TemplatePath { get; init; }

        /// <summary>
        /// The search string to match against the names of files in path.
        /// </summary>
        public string SearchPattern { get; init; } = "*";

        /// <inheritdoc/>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Path))
            {
                ThrowHelper.ThrowArgumentNullOrWhitespaceException(nameof(Path));
            }

            if (!Directory.Exists(Path))
            {
                ThrowHelper.ThrowDirectoryNotFoundException();
            }

            if (string.IsNullOrWhiteSpace(TemplatePath))
            {
                ThrowHelper.ThrowArgumentNullOrWhitespaceException(nameof(TemplatePath));
            }

            if (!File.Exists(TemplatePath))
            {
                ThrowHelper.ThrowFileNotFoundException(TemplatePath);
            }

            if (string.IsNullOrWhiteSpace(SearchPattern))
            {
                ThrowHelper.ThrowArgumentNullOrWhitespaceException(nameof(SearchPattern));
            }
        }
    }
}