using System.Diagnostics.CodeAnalysis;
using Stazor.Core;
using Stazor.Core.Helpers;

namespace Stazor.Plugins.IO
{
    public sealed class ReadFilesSettings : IValidatable
    {
        /// <summary>
        /// The relative or absolute path to the directory to search.
        /// </summary>
        [DisallowNull]
        public string? Path { get; init; }

        /// <summary>
        /// The relative or absolute path to the template directory.
        /// </summary>
        [DisallowNull]
        public string? TemplatePath { get; init; }

        /// <summary>
        /// The search string to match against the names of files in path.
        /// </summary>
        public string SearchPattern { get; init; } = "*.md";

        /// <inheritdoc/>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Path))
            {
                throw ThrowHelper.CreateArgumentNullOrWhitespaceException(nameof(Path));
            }

            if (string.IsNullOrWhiteSpace(TemplatePath))
            {
                throw ThrowHelper.CreateArgumentNullOrWhitespaceException(nameof(TemplatePath));
            }

            if (string.IsNullOrWhiteSpace(SearchPattern))
            {
                throw ThrowHelper.CreateArgumentNullOrWhitespaceException(nameof(SearchPattern));
            }
        }
    }
}