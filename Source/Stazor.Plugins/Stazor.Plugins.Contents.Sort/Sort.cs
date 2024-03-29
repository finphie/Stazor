﻿using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Stazor.Logging;

namespace Stazor.Plugins.Contents;

/// <summary>
/// ドキュメントをソートします。
/// </summary>
public sealed class Sort : IPostProcessingPlugin
{
    static readonly DocumentComparer Comparer = new();

    readonly IStazorLogger _logger;

    /// <summary>
    /// <see cref="Sort"/>クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="logger">ロガー</param>
    public Sort(IStazorLogger<Sort> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    /// <inheritdoc/>
    public void AfterExecute(IStazorDocument[] documents)
    {
        ArgumentNullException.ThrowIfNull(documents);

        _logger.Information("Start");

        // TODO: ソート処理を自前で実装する。
        Array.Sort(documents, Comparer);

        _logger.Information("End");
    }

    sealed class DocumentComparer : IComparer<IStazorDocument>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SuppressMessage("Style", "IDE0046:条件式に変換します", Justification = "可読性のため")]
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
