namespace Stazor.Themes;

/// <summary>
/// パイプラインは、プラグインの集合体で構成されます。
/// </summary>
public interface IPipeline
{
    /// <summary>
    /// パイプラインの処理を実行します。
    /// </summary>
    /// <param name="filePaths">ファイルパスの配列</param>
    /// <returns>ドキュメントの配列</returns>
    IStazorDocument[] Execute(string[] filePaths);
}
