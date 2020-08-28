using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Stazor.Core
{
    public interface IPlugin
    {
        IAsyncEnumerable<IDocument> ExecuteAsync(IAsyncEnumerable<IDocument> inputs);
    }

    
}