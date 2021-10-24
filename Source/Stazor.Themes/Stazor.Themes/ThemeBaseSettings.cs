using System.ComponentModel.DataAnnotations;

namespace Stazor.Themes;

/// <summary>
/// テーマの基本設定
/// </summary>
public abstract record ThemeBaseSettings
{
    /// <summary>
    /// コンテンツのパス
    /// </summary>
    [Required]
#pragma warning disable CS8618
    public string ContentPath { get; init; }
#pragma warning restore CS8618

    /// <summary>
    /// サイトタイトル
    /// </summary>
    [Required]
#pragma warning disable CS8618
    public string SiteTitle { get; init; }
#pragma warning restore CS8618

    /// <summary>
    /// コピーライト
    /// </summary>
    [Required]
#pragma warning disable CS8618
    public string Copyright { get; init; }
#pragma warning restore CS8618

    // slug
    // カテゴリ・タグ
}
