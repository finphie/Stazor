namespace Stazor.Core;

/// <summary>
/// ドキュメント
/// </summary>
/// <param name="TemplatePath">テンプレートパス</param>
/// <param name="Context">コンテキスト</param>
/// <param name="Metadata">メタデータ</param>
sealed record StazorDocument(string TemplatePath, IStazorContext Context, IStazorMetadata Metadata) : IStazorDocument;