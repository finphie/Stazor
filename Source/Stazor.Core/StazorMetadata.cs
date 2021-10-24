using System.ComponentModel.DataAnnotations;
using Utf8Utility;

namespace Stazor.Core;

/// <summary>
/// メタデータ
/// </summary>
/// <param name="Title">タイトル</param>
/// <param name="PublishedDate">公開日時</param>
/// <param name="ModifiedDate">更新日時</param>
/// <param name="Category">カテゴリー</param>
/// <param name="Tags">タグ</param>
sealed record StazorMetadata(
    [property: Required] Utf8Array Title,
    DateTimeOffset PublishedDate,
    DateTimeOffset ModifiedDate,
    [property: Required] string Category,
    [property: Required] IReadOnlySet<Utf8Array> Tags) : IStazorMetadata;
