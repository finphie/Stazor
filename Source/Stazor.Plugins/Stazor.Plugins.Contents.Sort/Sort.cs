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
        static readonly DocumentComparer Comparer = new();

        readonly IStazorLogger _logger;

        public Sort(IStazorLogger<Sort> logger)
            => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <inheritdoc/>
        public void AfterExecute(IStazorDocument[] documents)
        {
            _logger.Information("Start");
            // TODO: ソート処理を自前で実装する。
            Array.Sort(documents, Comparer);
            _logger.Information("End");
        }

        sealed class DocumentComparer : IComparer<IStazorDocument>
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