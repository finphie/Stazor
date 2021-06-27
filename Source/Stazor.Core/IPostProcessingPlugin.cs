using System.Threading.Tasks;

namespace Stazor.Core
{
    /// <summary>
    /// 後処理を定義するプラグイン
    /// </summary>
    public interface IPostProcessingPlugin : IPlugin
    {
        ValueTask AfterExecute(IDocumentList documents);
    }
}