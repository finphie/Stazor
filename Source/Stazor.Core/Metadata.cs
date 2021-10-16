namespace Stazor.Core;

/// <summary>
/// メタデータ作成クラスです。
/// </summary>
public static class Metadata
{
    /// <summary>
    /// メタデータを作成します。
    /// </summary>
    /// <param name="title">タイトル</param>
    /// <param name="publishedDate">公開日</param>
    /// <param name="modifiedDate">更新日</param>
    /// <param name="category">カテゴリー</param>
    /// <param name="tags">タグ</param>
    /// <returns>メタデータ</returns>
    public static IStazorMetadata Create(string title, DateTimeOffset publishedDate, DateTimeOffset modifiedDate, string category, IReadOnlySet<string> tags)
        => new StazorMetadata(title, publishedDate, modifiedDate, category, tags);
}