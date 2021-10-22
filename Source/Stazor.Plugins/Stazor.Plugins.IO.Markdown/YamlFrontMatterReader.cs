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

    public YamlFrontMatterReader(ReadOnlySpan<char> input)
    {
        _buffer = input;
        _position = 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryReadSeparator()
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryReadKeyValuePair(out ReadOnlySpan<char> key, out ReadOnlySpan<char> value)
    {
        SkipWhiteSpace();

        if (!TryReadKey(out key))
        {
            value = ReadOnlySpan<char>.Empty;
            return false;
        }

        SkipWhiteSpace();

        if (!TryReadValue(out value))
        {
            return false;
        }

        SkipWhiteSpaceAndNewLine();
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryReadKeyAndFlowStyleList(out ReadOnlySpan<char> key, [MaybeNullWhen(false)] out SortedSet<string> list)
    {
        SkipWhiteSpace();

        if (!TryReadKey(out key))
        {
            list = null;
            return false;
        }

        SkipWhiteSpace();

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
        var span = _buffer[_position..];
        var index = span.IndexOf(':');

        if (index <= 0)
        {
            key = ReadOnlySpan<char>.Empty;
            return false;
        }

        key = span[..index];
        _position += index;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool TryReadValue(out ReadOnlySpan<char> value)
    {
        var span = _buffer[_position..];
        var index = span.IndexOfAny('\n', ' ', '\r');

        if (index <= 0)
        {
            value = ReadOnlySpan<char>.Empty;
            return false;
        }

        value = span[..index];
        _position += index;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool TryReadFlowStyleList([MaybeNullWhen(false)] out SortedSet<string> list)
    {
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
}