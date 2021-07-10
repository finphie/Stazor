using System.Diagnostics.CodeAnalysis;
using Stazor.Core.Helpers;

namespace Stazor.Core
{
    public abstract record StazorBaseSettings// : IValidatable
    {
        /// <summary>
        /// サイトタイトル
        /// </summary>
        [DisallowNull]
        public string? SiteTitle { get; init; }

        [DisallowNull]
        public string? Copyright { get; init; }

        /// <inheritdoc/>
        public virtual void Validate()
        {
            if (string.IsNullOrWhiteSpace(SiteTitle))
            {
                ThrowHelper.ThrowArgumentNullOrWhitespaceException(nameof(SiteTitle));
            }

            if (string.IsNullOrWhiteSpace(Copyright))
            {
                ThrowHelper.ThrowArgumentNullOrWhitespaceException(nameof(Copyright));
            }
        }

        // slug
        // カテゴリ・タグ
    }
}