using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
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

        public void RenderTo(IBufferWriter<byte> bufferWriter, Dictionary<byte[], byte[]> content)
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
                        // TODO: パフォーマンス                     
                        content.TryGetValue(value.ToArray(), out var x);
                        bufferWriter.Write(x);
                        break;
                    default:
                        throw new InvalidEnumArgumentException(nameof(block.Type), (int)block.Type, typeof(BlockType));
                }              
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