using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Stazor.Logging;

namespace Stazor.Plugins.Contents
{
    /// <summary>
    /// Sorts the input documents.
    /// </summary>
    public sealed class Sort : IPostProcessingPlugin
    {
        readonly IStazorLogger _logger;

        public Sort(IStazorLogger<Sort> logger)
            => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <inheritdoc/>
        public void AfterExecute(IStazorDocument[] documents)
        {
            _logger.Information("Start");
            documents.AsSpan().Sort(new DocumentComparer());
            _logger.Information("End");
        }

        struct DocumentComparer : IComparer<IStazorDocument>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Compare(IStazorDocument? x, IStazorDocument? y)
            {
                if (x is null)
                {
                    return y is null ? 0 : -1;
                }

                if (y is null)
                {
                    return 1;
                }

                if (x.Metadata.PublishedDate == y.Metadata.PublishedDate)
                {
                    return 0;
                }

                return x.Metadata.PublishedDate > y.Metadata.PublishedDate ? -1 : 1;
            }
        }
    }
}