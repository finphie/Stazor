using System.Diagnostics.CodeAnalysis;
using Stazor.Core;
using Stazor.Core.Helpers;

namespace Stazor.Plugins.Renderer
{
    public sealed record MarkdownSettings : IStazorKey, IValidatable
    {
        /// <inheritdoc/>
        public string Key { get; init; } = nameof(Markdown);

        [DisallowNull]
        public string? InputKey { get; init; }

        /// <inheritdoc/>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Key))
            {
                ThrowHelper.ThrowArgumentNullOrWhitespaceException(nameof(Key));
            }

            if (string.IsNullOrWhiteSpace(InputKey))
            {
                ThrowHelper.ThrowArgumentNullOrWhitespaceException(nameof(InputKey));
            }
        }
    }
}