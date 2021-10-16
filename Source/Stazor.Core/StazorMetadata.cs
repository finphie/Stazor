using System.ComponentModel.DataAnnotations;

namespace Stazor.Core;

/// <summary>
/// メタデータ
/// </summary>
/// <param name="Title">タイトル</param>
/// <param name="PublishedDate">公開日時</param>
/// <param name="ModifiedDate">更新日時</param>
/// <param name="Category">カテゴリー</param>
/// <param name="Tags">タグ</param>
// TODO: internalに変更する。現状は、YAMLデシリアライズするためpublicにする必要あり。
public sealed record StazorMetadata(
    [property: Required] string Title,
    DateTimeOffset PublishedDate,
    DateTimeOffset ModifiedDate,
    [property: Required] string Category,
    [property: Required] IReadOnlySet<string> Tags) : IStazorMetadata;