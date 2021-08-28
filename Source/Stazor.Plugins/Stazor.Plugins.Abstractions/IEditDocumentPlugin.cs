using System.Diagnostics.CodeAnalysis;

namespace Stazor.Plugins;

/// <summary>
/// ドキュメント編集用プラグイン
/// </summary>
[SuppressMessage("Usage", "CA2252:This API requires opting into preview features", Justification = "アナライザーの誤検知(https://github.com/dotnet/roslyn-analyzers/issues/5366)")]
public interface IEditDocumentPlugin : IPlugin
{
    /// <summary>
    /// プラグインの処理を実行します。
    /// </summary>
    /// <param name="document">ドキュメント</param>
    void Execute(IStazorDocument document);
}