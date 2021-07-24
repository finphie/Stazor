using System.ComponentModel.DataAnnotations;
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
        [Required]
        [AllowNull]
        public string ContentPath { get; init; }

        /// <summary>
        /// サイトタイトル
        /// </summary>
        [Required]
        [AllowNull]
        public string SiteTitle { get; init; }

        /// <summary>
        /// コピーライト
        /// </summary>
        [Required]
        [AllowNull]
        public string Copyright { get; init; }

        // slug
        // カテゴリ・タグ
    }
}