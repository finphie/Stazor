namespace Stazor.Core
{
    /// <summary>
    /// ドキュメントは、テンプレートとコンテキスト、メタデータで構成されます。
    /// </summary>
    public interface IDocument
    {
        /// <summary>
        /// テンプレートのパスを取得します。
        /// </summary>
        /// <value>
        /// テンプレートパス
        /// </value>
        string TemplatePath { get; init; }

        /// <summary>
        /// コンテキストを取得します。
        /// </summary>
        /// <value>
        /// コンテキスト
        /// </value>
        IContext Context { get; init; }

        /// <summary>
        /// メタデータを取得します。
        /// </summary>
        /// <value>
        /// メタデータ
        /// </value>
        IMetadata Metadata { get; init; }
    }
}