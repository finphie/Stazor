namespace Stazor.Plugins
{
    /// <summary>
    /// ドキュメント編集用プラグイン
    /// </summary>
    public interface IEditDocumentPlugin : IPlugin
    {
        void Execute(IStazorDocument document);
    }
}