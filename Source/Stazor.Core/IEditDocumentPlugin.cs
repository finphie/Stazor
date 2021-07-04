namespace Stazor.Core
{
    /// <summary>
    /// ドキュメント編集用プラグイン
    /// </summary>
    public interface IEditDocumentPlugin : IPlugin
    {
        void Execute(IStazorDocument document);
    }
}