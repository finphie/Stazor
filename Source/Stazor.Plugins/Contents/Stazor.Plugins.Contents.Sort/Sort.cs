using System.Collections.Generic;
using System.Linq;
using Stazor.Core;

namespace Stazor.Plugins.Contents
{
    /// <summary>
    /// Sorts the input documents.
    /// </summary>
    public sealed class Sort : IPlugin
    {
        /// <inheritdoc/>
        public async IAsyncEnumerable<IDocument> ExecuteAsync(IAsyncEnumerable<IDocument> inputs)
        {
            var documents = await inputs.OrderBy(x => x.Metadata.PublishedDate).ToArrayAsync().ConfigureAwait(false);

            foreach (var document in documents)
            {
                yield return document;
            }
        }
    }
}