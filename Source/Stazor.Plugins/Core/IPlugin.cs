using System.Collections.Generic;
using Stazor.Core;

namespace Stazor.Plugins
{
    public interface IPlugin
    {
        IAsyncEnumerable<IDocument> ExecuteAsync(IAsyncEnumerable<IDocument> inputs);
    }
}