using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Stazor.Engines.Simple
{
    sealed class TemplateCache
    {
        public ReadOnlySpan<byte> Buffer => _buffer;

        public ReadOnlySpan<(BlockType Type, Range Range)> Blocks => CollectionsMarshal.AsSpan(_blocks);

        readonly byte[] _buffer;
        readonly List<ValueTuple<BlockType, Range>> _blocks;

        public TemplateCache(byte[] buffer)
        {
            _buffer = buffer;
            _blocks = new(16);

            Parse();
        }

        public void Debug()
        {
            var buffer = Buffer;

            foreach (ref readonly var block in Blocks)
            {
                var s = Encoding.UTF8.GetString(buffer[block.Range]);
                Console.WriteLine($"{block}: {s}");
            }
        }

        void Parse()
        {
            var reader = new HtmlReader(Buffer);

            while ((reader.TryRead(out var value) is var type) && type != BlockType.None)
            {
                _blocks.Add((type, value));
            }
        }
    }
}