using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Stazor.Engines.Simple
{
    ref struct HtmlReader
    {
        /// <summary>
        /// '{{'
        /// </summary>
        static ReadOnlySpan<byte> BeginObject => new byte[] { 0x7B, 0x7B };

        /// <summary>
        /// '}}'
        /// </summary>
        static ReadOnlySpan<byte> EndObject => new byte[] { 0x7D, 0x7D };

        readonly ReadOnlySpan<byte> _buffer;

        int _position;

        public HtmlReader(ReadOnlySpan<byte> input)
        {
            _buffer = input;
            _position = 0;
        }

        public BlockType TryRead(out Range range)
        {
            if (_position >= _buffer.Length)
            {
                range = default;
                return BlockType.None;
            }

            var span = _buffer.Slice(_position);

            if (span.StartsWith(BeginObject))
            {
                _position += 2;
                SkipWhitespace();

                var startPosition = _position;

                while (!TryFindIsEndObject())
                {
                    _position++;
                }

                if (startPosition == _position)
                {
                    range = startPosition..;
                    _position = _buffer.Length;
                    return BlockType.Html;
                }

                // trim whitespace
                range = _buffer[_position - 1] is (byte)' ' or (byte)'\t'
                    ? (startPosition..(_position - 1))
                    : (startPosition.._position);

                _position += 2;
                return BlockType.Object;
            }

            var begin = span.IndexOf(BeginObject);
            if (begin == -1)
            {
                range = _position..;
                _position = _buffer.Length;
                return BlockType.Html;
            }

            range = _position..begin;
            _position += begin;
            return BlockType.Html;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool TryFindIsEndObject()
        {
            if (_position + 1 < _buffer.Length)
            {
                ref var bufferStart = ref Unsafe.Add(ref MemoryMarshal.GetReference(_buffer), _position);
                var end = Unsafe.ReadUnaligned<ushort>(ref MemoryMarshal.GetReference(EndObject));

                return Unsafe.ReadUnaligned<ushort>(ref bufferStart) == end;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void SkipWhitespace()
        {
            ref var x = ref MemoryMarshal.GetReference(_buffer);

            while (_position < _buffer.Length)
            {
                if (Unsafe.Add(ref x, _position) is (byte)' ' or (byte)'\t')
                {
                    _position++;
                    continue;
                }

                return;
            }
        }
    }
}