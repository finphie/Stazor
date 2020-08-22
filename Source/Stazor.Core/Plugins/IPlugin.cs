using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stazor.Core.Plugins
{
    public interface IPlugin
    {
        IAsyncEnumerable<IDocument> ExecuteAsync(IAsyncEnumerable<IDocument> inputs);
    }
}