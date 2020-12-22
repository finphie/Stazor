using System.ComponentModel.DataAnnotations;

namespace Stazor.Core
{
    public abstract class StazorBaseSettings : ISettings
    {
        public const string Key = nameof(Stazor);

        /// <summary>
        /// サイトタイトル
        /// </summary>
        [Required]
        public string? SiteTitle { get; set; }

        [Required]
        public string? Copyright { get; set; }

        // slug
        // カテゴリ・タグ
    }

    public interface ISettings
    {

    }
}