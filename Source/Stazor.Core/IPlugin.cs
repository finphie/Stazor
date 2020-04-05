using System.Collections.Generic;

namespace Stazor.Core
{
    public interface IPlugin
    {
        IAsyncEnumerable<IDocument> ExecuteAsync(IAsyncEnumerable<IDocument> inputs);
    }
}