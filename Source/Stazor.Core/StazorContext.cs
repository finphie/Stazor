using System.Diagnostics.CodeAnalysis;
using Stazor.Core.Helpers;
using Utf8Utility;

namespace Stazor.Core;

/// <summary>
/// Dictionary型コンテキスト
/// </summary>
sealed class StazorContext : IStazorContext
{
    readonly IUtf8ArrayDictionary<Utf8Array> _symbols;

    /// <summary>
    /// <see cref="StazorContext"/>クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="symbols">識別子とUTF-8文字列のペアリスト</param>
    public StazorContext(IUtf8ArrayDictionary<Utf8Array> symbols)
        => _symbols = symbols ?? throw new ArgumentNullException(nameof(symbols));

    /// <inheritdoc/>
    public void Add(Utf8Array key, Utf8Array value)
    {
        if (!_symbols.TryAdd(key, value))
        {
            ThrowHelper.ThrowAddingDuplicateWithKeyArgumentException(key);
        }
    }

    /// <inheritdoc/>
    public bool TryGetValue(Utf8Array key, [NotNullWhen(true)] out Utf8Array value)
        => _symbols.TryGetValue(key, out value);

    /// <inheritdoc/>
    public bool TryGetValue(ReadOnlySpan<byte> key, [NotNullWhen(true)] out Utf8Array value)
        => _symbols.TryGetValue(key, out value);
}