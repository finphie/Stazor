using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
    /// <returns>
    /// 現在位置にセパレーターがある場合は<see langword="true"/>、
    /// それ以外の場合は<see langword="false"/>。
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TrySkipSeparator()
    {
        ReadOnlySpan<char> separator = "---";

        if (!_buffer[_position..].StartsWith(separator))
        {
            return false;
        }

        _position += separator.Length;
        SkipWhiteSpaceAndNewLine();

        return true;
    }

    /// <summary>
    /// キーと値を取得します。
    /// </summary>
    /// <param name="key">キー</param>
    /// <param name="value">値</param>
    /// <returns>
    /// 解析に成功した場合は<see langword="true"/>、
    /// 失敗した場合は<see langword="false"/>。
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryReadKeyValuePair(out ReadOnlySpan<char> key, out ReadOnlySpan<char> value)
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

    /// <summary>
    /// キーと日時を取得します。
    /// </summary>
    /// <param name="key">キー</param>
    /// <param name="dateTime">日時</param>
    /// <returns>
    /// 解析に成功した場合は<see langword="true"/>、
    /// 失敗した場合は<see langword="false"/>。
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryReadKeyAndDateTimeOffset(out ReadOnlySpan<char> key, out DateTimeOffset dateTime)
    {
        if (!TryReadKeyValuePair(out key, out var value))
        {
            dateTime = default;
            return false;
        }

        return DateTimeOffset.TryParse(value, out dateTime);
    }

    /// <summary>
    /// キーと文字列のリストを取得します。
    /// </summary>
    /// <param name="key">キー</param>
    /// <param name="list">文字列のリスト</param>
    /// <returns>
    /// 解析に成功した場合は<see langword="true"/>、
    /// 失敗した場合は<see langword="false"/>。
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryReadKeyAndFlowStyleList(out ReadOnlySpan<char> key, [MaybeNullWhen(false)] out SortedSet<string> list)
    {
        if (!TryReadKey(out key))
        {
            list = null;
            return false;
        }

        if (!TryReadFlowStyleList(out list))
        {
            return false;
        }

        SkipWhiteSpaceAndNewLine();
        return true;
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

        // +1は「:」の読み飛ばし
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
    bool TryReadFlowStyleList([MaybeNullWhen(false)] out SortedSet<string> list)
    {
        SkipWhiteSpace();

        var end = _buffer[_position..].IndexOf(']');

        if (_buffer[_position] != '[' || end <= 0)
        {
            list = null;
            return false;
        }

        SkipWhiteSpace();
        list = new();

        while (_position < _buffer.Length)
        {
            var span = _buffer[_position..end];
            var index = span.IndexOf(',');

            if (index <= 0)
            {
                break;
            }

            var value = span[..index];
            _position += index;

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