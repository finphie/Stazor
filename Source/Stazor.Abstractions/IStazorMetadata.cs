namespace Stazor;

/// <summary>
/// メタデータ
/// </summary>
public interface IStazorMetadata
{
    /// <summary>
    /// タイトルを取得または設定します。
    /// </summary>
    /// <value>
    /// タイトル
    /// </value>
    string? Title { get; set; }

    /// <summary>
    /// 公開日を取得または設定します。
    /// </summary>
    /// <value>
    /// 公開日
    /// </value>
    DateTimeOffset PublishedDate { get; set; }

    /// <summary>
    /// 更新日を取得または設定します。
    /// </summary>
    /// <value>
    /// 更新日
    /// </value>
    DateTimeOffset ModifiedDate { get; set; }

    /// <summary>
    /// カテゴリー名を取得または設定します。
    /// </summary>
    /// <value>
    /// カテゴリー名
    /// </value>
    string? Category { get; set; }

    /// <summary>
    /// タグ名を取得または設定します。
    /// </summary>
    /// <value>
    /// タグ
    /// </value>
    IReadOnlySet<string> Tags { get; set; }
}