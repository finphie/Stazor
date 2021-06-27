using System.Threading.Tasks;

namespace Stazor.Core
{
    /// <summary>
    /// ドキュメント新規作成用プラグイン
    /// </summary>
    public interface INewDocumentsPlugin : IPlugin
    {
        ValueTask<IDocumentList> CreateDocumentsAsync();
    }
}