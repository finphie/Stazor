using System;
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
        readonly IStazorLogger _logger;

        public Sort(IStazorLogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<IDocument> ExecuteAsync(IAsyncEnumerable<IDocument> inputs)
        {
            _logger.Information("Start");

            var documents = await inputs.OrderBy(x => x.Metadata.PublishedDate).ToArrayAsync().ConfigureAwait(false);

            foreach (var document in documents)
            {
                yield return document;
            }

            _logger.Information("End");
        }
    }
}