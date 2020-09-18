using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Stazor.Engines.Simple.Extensions;
using Stazor.Engines.Simple.Helpers;
using static Stazor.Engines.Simple.HtmlParserException;

[assembly: InternalsVisibleTo("Stazor.Tests.Engines.Simple")]

namespace Stazor.Engines.Simple
{
    ref struct HtmlReader
    {
        /// <summary>
        /// '{{'
        /// </summary>
        const ushort StartObject = 0x7b7b;

        /// <summary>
        /// '}}'
        /// </summary>
        const ushort EndObject = 0x7d7d;

        readonly ReadOnlySpan<byte> _buffer;

        int _position;

        public HtmlReader(ReadOnlySpan<byte> input)
        {
            _buffer = input;
            _position = 0;
        }

        public BlockType Read(out Range range)
        {
            if (_position >= _buffer.Length)
            {
                range = default;
                return BlockType.None;
            }

            if (TryReadHtml(out range))
            {
                return BlockType.Html;
            }

            ReadObject(out range);           
            return BlockType.Object;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryReadHtml(out Range range)
        {
            if (_position >= _buffer.Length)
            {
                range = default;
                return false;
            }

            var startPosition = _position;
            ref var bufferStart = ref MemoryMarshal.GetReference(_buffer);

            while (_position + 1 < _buffer.Length)
            {
                // '{{'で始まらない場合
                if (!IsStartObjectInternal(ref bufferStart))
                {
                    _position++;
                    continue;
                }

                // 出力対象文字が1文字もない場合
                if (startPosition == _position)
                {
                    // TODO
                    range = default;
                    return false;
                }

                range = startPosition.._position;
                return true;
            }

            range = startPosition..++_position;
            return true;
        }

        public void ReadObject(out Range range)
        {
            // TODO: 改行を考慮する。

            if (_position >= _buffer.Length)
            {
                ThrowHelper.ThrowHtmlParserException(ParserError.ExpectedStartObject, _position);
            }

            ref var bufferStart = ref MemoryMarshal.GetReference(_buffer);

            // '{{'で始まらない場合
            if (!IsStartObjectInternal(ref bufferStart))
            {
                // 2文字の内、最初の1文字が'{'の場合
                if (_position + 1 < _buffer.Length && Unsafe.Add(ref bufferStart, _position) == (byte)'{')
                {
                    _position++;
                }

                ThrowHelper.ThrowHtmlParserException(ParserError.ExpectedStartObject, _position);
            }

            _position += sizeof(ushort);

            if (_position >= _buffer.Length)
            {
                ThrowHelper.ThrowHtmlParserException(ParserError.ExpectedEndObject, --_position);
            }

            // '{'が3つ以上連続している場合
            if (Unsafe.Add(ref bufferStart, _position) == (byte)'{')
            {
                ThrowHelper.ThrowHtmlParserException(ParserError.ExpectedStartObject, _position);
            }

            // '{'に連続するスペースを削除
            SkipWhiteSpaceInternal(ref bufferStart);

            // スペースを削除後、bufferの末尾に到達した場合
            if (_position >= _buffer.Length)
            {
                ThrowHelper.ThrowHtmlParserException(ParserError.ExpectedEndObject, --_position);
            }

            var startPosition = _position;

            while (_position + 1 < _buffer.Length)
            {
                // '}}'で終わる場合
                if (IsEndObjectInternal(ref bufferStart))
                {                   
                    var endPosition = _position;

                    // '{{'と'}}'の間に1文字もない場合
                    if (startPosition == endPosition)
                    {
                        ThrowHelper.ThrowHtmlParserException(ParserError.InvalidObjectFormat, _position);
                    }

                    // 末尾のスペースを削除
                    for (; endPosition - 1 > startPosition; endPosition--)
                    {
                        if (!Unsafe.Add(ref bufferStart, endPosition - 1).IsWhiteSpace())
                        {
                            break;
                        }
                    }

                    _position += sizeof(ushort);

                    // '}'が3つ以上連続している場合
                    if (_position < _buffer.Length && Unsafe.Add(ref bufferStart, _position) == (byte)'}')
                    {
                        ThrowHelper.ThrowHtmlParserException(ParserError.ExpectedEndObject, _position);
                    }

                    range = startPosition..endPosition;
                    return;
                }

                _position++;
            }

            ThrowHelper.ThrowHtmlParserException(ParserError.ExpectedEndObject, _position);
            range = default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsStartObject()
            => IsStartObjectInternal(ref MemoryMarshal.GetReference(_buffer));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEndObject()
            => IsEndObjectInternal(ref MemoryMarshal.GetReference(_buffer));    

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SkipWhiteSpace()
            => SkipWhiteSpaceInternal(ref MemoryMarshal.GetReference(_buffer));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool IsStartObjectInternal(ref byte bufferStart)
        {
            Debug.Assert(_buffer[0] == bufferStart);
            return _position + 1 < _buffer.Length && Unsafe.ReadUnaligned<ushort>(ref Unsafe.Add(ref bufferStart, _position)) == StartObject;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool IsEndObjectInternal(ref byte bufferStart)
        {
            Debug.Assert(_buffer[0] == bufferStart);
            return _position + 1 < _buffer.Length && Unsafe.ReadUnaligned<ushort>(ref Unsafe.Add(ref bufferStart, _position)) == EndObject;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void SkipWhiteSpaceInternal(ref byte bufferStart)
        {
            Debug.Assert(_buffer[0] == bufferStart);

            while (_position < _buffer.Length)
            {
                if (!Unsafe.Add(ref bufferStart, _position).IsWhiteSpace())
                {
                    return;
                }

                _position++;
                continue;
            }
        }
    }
}