using System.Diagnostics.CodeAnalysis;
using Stazor.Core;
using Stazor.Core.Helpers;

namespace Stazor.Plugins.Metadata
{
    public sealed record StyleSheetSettings : IStazorKey, IValidatable
    {
        /// <inheritdoc/>
        public string Key { get; init; } = nameof(StyleSheet);

        /// <summary>
        /// The CSS file url.
        /// </summary>
        [DisallowNull]
        public string? Href { get; init; }

        /// <inheritdoc/>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Key))
            {
                ThrowHelper.ThrowArgumentNullOrWhitespaceException(nameof(Key));
            }

            // TODO: ファイル存在チェック
            if (string.IsNullOrWhiteSpace(Href))
            {
                ThrowHelper.ThrowArgumentNullOrWhitespaceException(nameof(Href));
            }
        }
    }
}