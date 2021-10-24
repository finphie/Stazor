namespace Stazor.Plugins;

/// <summary>
/// ドキュメント新規作成用プラグイン
/// </summary>
public interface INewDocumentsPlugin : IPlugin
{
    /// <summary>
    /// プラグインの処理を実行します。
    /// </summary>
    /// <param name="filePath">ファイルパス</param>
    /// <returns>新規作成されたドキュメント</returns>
    IStazorDocument CreateDocument(string filePath);
}
