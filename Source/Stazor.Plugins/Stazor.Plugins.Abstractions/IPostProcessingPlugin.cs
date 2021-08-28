using System.Diagnostics.CodeAnalysis;

namespace Stazor.Plugins;

/// <summary>
/// 後処理を行うプラグイン
/// </summary>
[SuppressMessage("Usage", "CA2252:This API requires opting into preview features", Justification = "アナライザーの誤検知(https://github.com/dotnet/roslyn-analyzers/issues/5366)")]
public interface IPostProcessingPlugin : IPlugin
{
    /// <summary>
    /// プラグインの処理を実行します。
    /// </summary>
    /// <param name="documents">ドキュメントの配列</param>
    void AfterExecute(IStazorDocument[] documents);
}