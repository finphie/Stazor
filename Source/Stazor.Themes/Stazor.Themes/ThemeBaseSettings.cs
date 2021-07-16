using System.Diagnostics.CodeAnalysis;

namespace Stazor.Themes
{
    public abstract record ThemeBaseSettings
    {
        public string ContentPath { get; set; }

        /// <summary>
        /// サイトタイトル
        /// </summary>
        [DisallowNull]
        public string? SiteTitle { get; set; }

        [DisallowNull]
        public string? Copyright { get; set; }


        // slug
        // カテゴリ・タグ
    }
}