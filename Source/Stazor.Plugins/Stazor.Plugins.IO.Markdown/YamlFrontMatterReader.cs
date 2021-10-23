using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Stazor.Plugins.IO.Helpers;

namespace Stazor.Plugins.IO;

/// <summary>
/// Front-matterの簡易的な解析を行う構造体です。
/// </summary>
ref struct YamlFrontMatterReader
{
    readonly ReadOnlySpan<char> _buffer;
    int _position;

    /// <summary>
    /// <see cref="YamlFrontMatterReader"/>構造体の新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="input">UTF-16文字列</param>
    public YamlFrontMatterReader(ReadOnlySpan<char> input)
    {
        _buffer = input;
        _position = 0;
    }

    /// <summary>
    /// セパレーターをスキップします。
    /// </summary>
    /// <exception cref="YamlParserException">解析に失敗した場合はこの例外をスローします。</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SkipSeparator()
    {
        ReadOnlySpan<char> separator = "---";

        if (!_buffer[_position..].StartsWith(separator))
        {
            ThrowHelper.ThrowYamlParserException(_position);
        }

        _position += separator.Length;
        SkipWhiteSpaceAndNewLine();
    }

    /// <summary>
    /// キーと文字列を取得します。
    /// </summary>
    /// <param name="key">キー</param>
    /// <returns>
    /// 文字列を返します。
    /// </returns>
    /// <exception cref="YamlParserException">解析に失敗した場合はこの例外をスローします。</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ReadKeyAndString(out ReadOnlySpan<char> key)
    {
        if (!TryReadKeyValuePair(out key, out var value))
        {
            ThrowHelper.ThrowYamlParserException(_position);
        }

        return value.ToString();
    }

    /// <summary>
    /// キーと日時を取得します。
    /// </summary>
    /// <param name="key">キー</param>
    /// <returns>
    /// 日時を返します。
    /// </returns>
    /// <exception cref="YamlParserException">解析に失敗した場合はこの例外をスローします。</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateTimeOffset ReadKeyAndDateTimeOffset(out ReadOnlySpan<char> key)
    {
        if (!TryReadKeyValuePair(out key, out var value))
        {
            ThrowHelper.ThrowYamlParserException(_position);
        }

        return DateTimeOffset.Parse(value);
    }

    /// <summary>
    /// キーと文字列のリストを取得します。
    /// </summary>
    /// <param name="key">キー</param>
    /// <returns>
    /// 文字列のリストを返します。
    /// </returns>
    /// <exception cref="YamlParserException">解析に失敗した場合はこの例外をスローします。</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SortedSet<string> ReadKeyAndFlowStyleList(out ReadOnlySpan<char> key)
    {
        if (!TryReadKey(out key))
        {
            ThrowHelper.ThrowYamlParserException(_position);
        }

        if (!TryReadFlowStyleList(out var list))
        {
            ThrowHelper.ThrowYamlParserException(_position);
        }

        SkipWhiteSpaceAndNewLine();
        return list;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool TryReadKey(out ReadOnlySpan<char> key)
    {
        SkipWhiteSpace();

        var span = _buffer[_position..];
        var index = span.IndexOf(':');

        if (index <= 0)
        {
            key = ReadOnlySpan<char>.Empty;
            return false;
        }

        key = span[..index];

        // +1は':'の読み飛ばし
        _position += index + 1;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool TryReadValue(out ReadOnlySpan<char> value)
    {
        SkipWhiteSpace();
        SkipSingleQuotation();

        var span = _buffer[_position..];
        var index = span.IndexOfAny('\n', ' ', '\r');

        if (index <= 0)
        {
            value = ReadOnlySpan<char>.Empty;
            return false;
        }

        if (span[index - 1] == '\'')
        {
            index--;
        }

        SkipSingleQuotation();

        value = span[..index];
        _position += index;

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool TryReadKeyValuePair(out ReadOnlySpan<char> key, out ReadOnlySpan<char> value)
    {
        if (!TryReadKey(out key))
        {
            value = ReadOnlySpan<char>.Empty;
            return false;
        }

        if (!TryReadValue(out value))
        {
            return false;
        }

        SkipWhiteSpaceAndNewLine();
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool TryReadFlowStyleList([MaybeNullWhen(false)] out SortedSet<string> list)
    {
        SkipWhiteSpace();

        var end = _buffer[_position..].IndexOf(']');

        if (_buffer[_position] != '[' || end <= 0)
        {
            list = null;
            return false;
        }

        _position++;
        list = new();

        // -1は']'の除外
        end += _position - 1;

        while (_position < _buffer.Length)
        {
            SkipWhiteSpace();

            var span = _buffer[_position..end];
            var index = span.IndexOf(',');

            if (index <= 0)
            {
                list.Add(span.TrimEnd(' ').ToString());
                break;
            }

            var value = span[..index];
            _position += index + 1;

            list.Add(value.ToString());
        }

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void SkipWhiteSpaceAndNewLine()
    {
        ref var bufferStart = ref MemoryMarshal.GetReference(_buffer);

        while (_position < _buffer.Length)
        {
            var token = Unsafe.Add(ref bufferStart, (nint)(uint)_position);

            if (token is not ' ' and not '\n' and not '\r')
            {
                break;
            }

            _position++;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void SkipWhiteSpace()
    {
        ref var bufferStart = ref MemoryMarshal.GetReference(_buffer);

        while (_position < _buffer.Length)
        {
            if (Unsafe.Add(ref bufferStart, (nint)(uint)_position) != ' ')
            {
                break;
            }

            _position++;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void SkipSingleQuotation()
    {
        ref var bufferStart = ref MemoryMarshal.GetReference(_buffer);

        if (_position < _buffer.Length && Unsafe.Add(ref bufferStart, (nint)(uint)_position) == '\'')
        {
            _position++;
        }
    }
}