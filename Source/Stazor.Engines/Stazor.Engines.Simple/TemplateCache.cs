using System;
using System.Buffers;
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

        public void RenderTo(IBufferWriter<byte> bufferWriter, Dictionary<string, byte[]> content)
        {
            var buffer = Buffer;

            foreach (ref readonly var block in Blocks)
            {
                var value = buffer[block.Range];

                switch (block.Type)
                {
                    case BlockType.Html:
                        bufferWriter.Write(value);
                        break;
                    case BlockType.Object:
                        bufferWriter.Write(content[Encoding.UTF8.GetString(value)]);
                        break;
                    default:
                        throw new Exception();
                }

                var s = Encoding.UTF8.GetString(buffer[block.Range]);
                Console.WriteLine($"{block}: {s}");
            }
        }

        void Parse()
        {
            var reader = new HtmlReader(Buffer);

            while ((reader.Read(out var value) is var type) && type != BlockType.None)
            {
                _blocks.Add((type, value));
            }
        }
    }
}