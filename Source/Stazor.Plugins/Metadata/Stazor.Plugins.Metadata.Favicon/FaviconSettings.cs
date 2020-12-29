using System.Diagnostics.CodeAnalysis;
using Stazor.Core;
using Stazor.Core.Helpers;

namespace Stazor.Plugins.Metadata
{
    public sealed class FaviconSettings : IStazorSettings, IValidatable
    {
        /// <summary>
        /// The favicon url.
        /// </summary>
        [DisallowNull]
        public string? Href { get; init; }

        /// <inheritdoc/>
        public void Validate()
        {
            // TODO: ファイル存在チェック
            if (string.IsNullOrWhiteSpace(Href))
            {
                ThrowHelper.ThrowArgumentNullOrWhitespaceException(nameof(Href));
            }
        }
    }
}