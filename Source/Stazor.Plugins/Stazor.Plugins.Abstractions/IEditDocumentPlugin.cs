namespace Stazor.Plugins;

/// <summary>
/// ドキュメント編集用プラグイン
/// </summary>
public interface IEditDocumentPlugin : IPlugin
{
    /// <summary>
    /// プラグインの処理を実行します。
    /// </summary>
    /// <param name="document">ドキュメント</param>
    void Execute(IStazorDocument document);
}
