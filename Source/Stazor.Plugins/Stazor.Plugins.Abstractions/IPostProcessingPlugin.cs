namespace Stazor.Plugins;

/// <summary>
/// 後処理を行うプラグイン
/// </summary>
public interface IPostProcessingPlugin : IPlugin
{
    /// <summary>
    /// プラグインの処理を実行します。
    /// </summary>
    /// <param name="documents">ドキュメントの配列</param>
    void AfterExecute(IStazorDocument[] documents);
}