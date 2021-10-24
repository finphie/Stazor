using Utf8Utility;

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
    Utf8Array Title { get; init; }

    /// <summary>
    /// 公開日を取得または設定します。
    /// </summary>
    /// <value>
    /// 公開日
    /// </value>
    DateTimeOffset PublishedDate { get; init; }

    /// <summary>
    /// 更新日を取得または設定します。
    /// </summary>
    /// <value>
    /// 更新日
    /// </value>
    DateTimeOffset ModifiedDate { get; init; }

    /// <summary>
    /// カテゴリーを取得または設定します。
    /// </summary>
    /// <value>
    /// カテゴリー
    /// </value>
    string Category { get; init; }

    /// <summary>
    /// タグを取得または設定します。
    /// </summary>
    /// <value>
    /// タグ
    /// </value>
    IReadOnlySet<Utf8Array> Tags { get; init; }
}
