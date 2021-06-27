using System.Threading.Tasks;

namespace Stazor.Core
{
    /// <summary>
    /// ドキュメント編集用プラグイン
    /// </summary>
    public interface IEditDocumentPlugin : IPlugin
    {
        ValueTask ExecuteAsync(IStazorDocument document);
    }
}