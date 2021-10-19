namespace Stazor;

/// <summary>
/// ドキュメントは、テンプレートとコンテキスト及びメタデータで構成されます。
/// </summary>
public interface IStazorDocument
{
    /// <summary>
    /// テンプレートファイルのパスを取得します。
    /// </summary>
    /// <value>
    /// テンプレートファイルのパス
    /// </value>
    string TemplatePath { get; init; }

    /// <summary>
    /// コンテキストを取得します。
    /// </summary>
    /// <value>
    /// コンテキスト
    /// </value>
    IStazorContext Context { get; init; }

    /// <summary>
    /// メタデータを取得します。
    /// </summary>
    /// <value>
    /// メタデータ
    /// </value>
    IStazorMetadata Metadata { get; init; }
}