using System.Collections.Generic;

namespace Stazor.Core
{
    /// <summary>
    /// Represents the plugin interface.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Executes the job.
        /// </summary>
        /// <param name="inputs">The document sequence.</param>
        /// <returns>Returns the document sequence.</returns>
        IAsyncEnumerable<IDocument> ExecuteAsync(IAsyncEnumerable<IDocument> inputs);
    }
}