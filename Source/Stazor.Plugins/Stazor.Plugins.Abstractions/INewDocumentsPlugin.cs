using System.Diagnostics.CodeAnalysis;

namespace Stazor.Plugins;

/// <summary>
/// ドキュメント新規作成用プラグイン
/// </summary>
[SuppressMessage("Usage", "CA2252:This API requires opting into preview features", Justification = "アナライザーの誤検知(https://github.com/dotnet/roslyn-analyzers/issues/5366)")]
public interface INewDocumentsPlugin : IPlugin
{
    /// <summary>
    /// プラグインの処理を実行します。
    /// </summary>
    /// <param name="filePath">ファイルパス</param>
    /// <returns>新規作成されたドキュメント</returns>
    IStazorDocument CreateDocument(string filePath);
}