using Stazor.Core.Helpers;

namespace Stazor.Core
{
    public abstract class StazorBaseSettings : IStazorSettings, IValidatable
    {
        public const string Key = nameof(Stazor);

        /// <summary>
        /// サイトタイトル
        /// </summary>
        public string? SiteTitle { get; set; }

        public string? Copyright { get; set; }

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