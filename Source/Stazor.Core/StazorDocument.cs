using System.ComponentModel.DataAnnotations;

namespace Stazor.Core;

/// <summary>
/// ドキュメント
/// </summary>
/// <param name="TemplatePath">テンプレートファイルのパス</param>
/// <param name="Context">コンテキスト</param>
/// <param name="Metadata">メタデータ</param>
sealed record StazorDocument(
    [property: Required] string TemplatePath,
    [property: Required] IStazorContext Context,
    [property: Required] IStazorMetadata Metadata) : IStazorDocument;