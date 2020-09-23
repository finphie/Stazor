using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Stazor.Engines.Simple
{
    /// <summary>
    /// A cache class to use when parsing HTML.
    /// </summary>
    sealed class TemplateCache
    {
        readonly byte[] _buffer;
        readonly List<ValueTuple<BlockType, Range>> _blocks;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateCache"/> class.
        /// </summary>
        /// <param name="buffer">UTF-8 encoded buffer.</param>
        public TemplateCache(byte[] buffer)
        {
            _buffer = buffer;
            _blocks = new(16);

            Parse();
        }

        /// <summary>
        /// Gets a UTF-8 encoded buffer.
        /// </summary>
        /// <value>
        /// UTF-8 encoded buffer.
        /// </value>
        public ReadOnlySpan<byte> Buffer => _buffer;

        /// <summary>
        /// Gets a UTF-8 encoded block buffer.
        /// </summary>
        /// <value>
        /// The block buffer.
        /// </value>
        internal ReadOnlySpan<(BlockType Type, Range Range)> Blocks => CollectionsMarshal.AsSpan(_blocks);

        /// <summary>
        /// Renders string content and writes it to the <see cref="IBufferWriter{Byte}" />.
        /// </summary>
        /// <param name="bufferWriter">The target writer.</param>
        /// <param name="content">The content.</param>
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