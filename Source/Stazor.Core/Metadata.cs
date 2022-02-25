using Stazor.Core.Helpers;

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
    /// <param name="publishedDate">公開日時</param>
    /// <param name="modifiedDate">更新日時</param>
    /// <param name="category">カテゴリー</param>
    /// <param name="tags">タグ</param>
    /// <returns>メタデータ</returns>
    public static IStazorMetadata Create(string title, DateTimeOffset publishedDate, DateTimeOffset modifiedDate, string category, IReadOnlySet<string> tags!!)
    {
        ThrowHelper.ThrowArgumentNullOrWhitespaceExceptionIfNullOrWhitespace(title);
        ThrowHelper.ThrowArgumentNullOrWhitespaceExceptionIfNullOrWhitespace(category);

        if (publishedDate > modifiedDate)
        {
            ThrowHelper.ThrowInvalidDateException(nameof(modifiedDate));
        }

        return new StazorMetadata(title, publishedDate, modifiedDate, category, tags);
    }
}
