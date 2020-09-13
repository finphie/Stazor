using System;

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
                var end = span.IndexOf(EndObject);
                if (end == -1)
                {
                    range = _position..;
                    _position = _buffer.Length;
                    return BlockType.Html;
                }

                range = (_position + 2)..(_position + end);
                _position += end + 2;
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
    }
}