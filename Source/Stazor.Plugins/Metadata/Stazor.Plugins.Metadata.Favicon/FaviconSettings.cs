using System.Diagnostics.CodeAnalysis;
using System.IO;
using Stazor.Core;
using Stazor.Core.Helpers;

namespace Stazor.Plugins.Metadata
{
    public sealed record FaviconSettings : IStazorKey, IValidatable
    {
        /// <inheritdoc/>
        public string Key { get; init; } = nameof(Favicon);

        /// <summary>
        /// The favicon url.
        /// </summary>
        [DisallowNull]
        public string? FilePath { get; init; }

        /// <inheritdoc/>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Key))
            {
                ThrowHelper.ThrowArgumentNullOrWhitespaceException(nameof(Key));
            }

            if (string.IsNullOrWhiteSpace(FilePath))
            {
                ThrowHelper.ThrowArgumentNullOrWhitespaceException(nameof(FilePath));
            }

            if (!File.Exists(FilePath))
            {
                ThrowHelper.ThrowFileNotFoundException(FilePath);
            }
        }
    }
}