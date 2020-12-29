using System.Diagnostics.CodeAnalysis;
using Stazor.Core;
using Stazor.Core.Helpers;

namespace Stazor.Plugins.Renderer
{
    public sealed class MarkdownSettings : IStazorSettings, IValidatable
    {
        [DisallowNull]
        public string? InputKey { get; init; }

        /// <inheritdoc/>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(InputKey))
            {
                ThrowHelper.ThrowArgumentNullOrWhitespaceException(nameof(InputKey));
            }
        }
    }
}