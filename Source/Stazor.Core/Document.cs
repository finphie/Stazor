namespace Stazor.Core
{
    /// <summary>
    /// ドキュメント
    /// </summary>
    /// <param name="TemplatePath">テンプレートパス</param>
    /// <param name="Context">コンテキスト</param>
    /// <param name="Metadata">メタデータ</param>
    public sealed record Document(string TemplatePath, IContext Context, IMetadata Metadata) : IDocument;
}