using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Stazor.Engines.Simple.Extensions;

[assembly: InternalsVisibleTo("Stazor.Tests.Engines.Simple")]

namespace Stazor.Engines.Simple
{
    ref struct HtmlReader
    {
        /// <summary>
        /// '{{'
        /// </summary>
        const ushort BeginObject = 0x7b7b;

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

            if (TryReadObject(out range))
            {
                return BlockType.Object;
            }

            ReadHtml(out range);           
            return BlockType.Html;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadHtml(out Range range)
        {
            if (_position >= _buffer.Length)
            {
                range = default;
                return;
            }

            var position = _position;
            ref var bufferStart = ref MemoryMarshal.GetReference(_buffer);

            while (_position + 1 < _buffer.Length)
            {
                if (Unsafe.ReadUnaligned<ushort>(ref Unsafe.Add(ref bufferStart, _position)) == BeginObject)
                {
                    range = position.._position;
                    return;
                }

                _position++;
            }

            range = position..++_position;
        }

        public bool TryReadObject(out Range range)
        {
            if (_position >= _buffer.Length)
            {
                goto END;
            }

            ref var bufferStart = ref MemoryMarshal.GetReference(_buffer);

            // '{{'で始まらない場合
            if (_position + 1 >= _buffer.Length || Unsafe.ReadUnaligned<ushort>(ref Unsafe.Add(ref bufferStart, _position)) != BeginObject)
            {
                goto END;
            }

            _position += sizeof(ushort);

            // '{'が3つ以上連続している場合
            if (_position >= _buffer.Length || Unsafe.Add(ref bufferStart, _position) == (byte)'{')
            {
                goto END;
            }

            // '{'に連続するスペースを削除
            SkipWhiteSpace();

            var startPosition = _position;

            while (_position + 1 < _buffer.Length)
            {
                // '}}'で終わる場合
                if (Unsafe.ReadUnaligned<ushort>(ref Unsafe.Add(ref bufferStart, _position)) == EndObject)
                {
                    // 末尾のスペースを削除
                    var endPosition = _position;
                    for (; endPosition - 1 > startPosition; endPosition--)
                    {
                        if (!Unsafe.Add(ref bufferStart, endPosition - 1).IsWhiteSpace())
                        {
                            break;
                        }
                    }

                    _position += sizeof(ushort);

                    // '}'が3つ以上連続している場合
                    //if (_position < _buffer.Length && Unsafe.Add(ref bufferStart, _position) == (byte)'}')
                    //{
                    //    goto END;
                    //}

                    range = startPosition..endPosition;
                    return true;
                }

                _position++;
            }

            END:
            range = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void SkipWhiteSpace()
        {
            ref var bufferStart = ref MemoryMarshal.GetReference(_buffer);

            while (_position < _buffer.Length)
            {
                if (Unsafe.Add(ref bufferStart, _position).IsWhiteSpace())
                {
                    _position++;
                    continue;
                }

                return;
            }
        }
    }
}