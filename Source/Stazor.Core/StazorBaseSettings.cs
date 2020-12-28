using System.Diagnostics.CodeAnalysis;
using Stazor.Core.Helpers;

namespace Stazor.Core
{
    public abstract class StazorBaseSettings : IStazorSettings, IValidatable
    {
        public const string Key = nameof(Stazor);

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
                throw ThrowHelper.CreateArgumentNullOrWhitespaceException(nameof(SiteTitle));
            }

            if (string.IsNullOrWhiteSpace(Copyright))
            {
                throw ThrowHelper.CreateArgumentNullOrWhitespaceException(nameof(Copyright));
            }
        }

        // slug
        // カテゴリ・タグ
    }
}