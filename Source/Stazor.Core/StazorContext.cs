using System.Diagnostics.CodeAnalysis;
using Stazor.Core.Helpers;
using Utf8Utility;

namespace Stazor.Core;

/// <summary>
/// Dictionary型コンテキスト
/// </summary>
sealed class StazorContext : IStazorContext
{
    readonly IUtf8StringDictionary<Utf8String> _symbols;

    /// <summary>
    /// <see cref="StazorContext"/>クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="symbols">識別子とUTF-8文字列のペアリスト</param>
    public StazorContext(IUtf8StringDictionary<Utf8String> symbols)
    {
        if (symbols is null)
        {
            ThrowHelper.ThrowArgumentNullException(nameof(symbols));
        }

        _symbols = symbols;
    }

    /// <inheritdoc/>
    public void Add(Utf8String key, Utf8String value)
    {
        if (!_symbols.TryAdd(key, value))
        {
            ThrowHelper.ThrowAddingDuplicateWithKeyArgumentException(key);
        }
    }

    /// <inheritdoc/>
    public bool TryGetValue(Utf8String key, [NotNullWhen(true)] out Utf8String value)
        => _symbols.TryGetValue(key, out value);

    /// <inheritdoc/>
    public bool TryGetValue(ReadOnlySpan<byte> key, [NotNullWhen(true)] out Utf8String value)
        => _symbols.TryGetValue(key, out value);
}