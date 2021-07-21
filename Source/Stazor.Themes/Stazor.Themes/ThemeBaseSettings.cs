using System.Diagnostics.CodeAnalysis;

namespace Stazor.Themes
{
    /// <summary>
    /// テーマの基本設定
    /// </summary>
    public abstract record ThemeBaseSettings
    {
        /// <summary>
        /// コンテンツのパス
        /// </summary>
        public string ContentPath { get; set; }

        /// <summary>
        /// サイトタイトル
        /// </summary>
        [DisallowNull]
        public string? SiteTitle { get; set; }

        /// <summary>
        /// コピーライト
        /// </summary>
        [DisallowNull]
        public string? Copyright { get; set; }

        // slug
        // カテゴリ・タグ
    }
}