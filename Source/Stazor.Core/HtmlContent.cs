using System.Buffers;

namespace Stazor.Core
{
    public sealed class HtmlContent
    {
        /// <summary>
        /// HTML Head
        /// </summary>
        public ArrayBufferWriter<byte> Head { get; } = new();

        /// <summary>
        /// HTML Body
        /// </summary>
        public HtmlBody Body { get; } = new();
    }

    public sealed class HtmlBody
    {
        public ArrayBufferWriter<byte> Header { get; } = new();

        public HtmlMain Main { get; } = new();

        public ArrayBufferWriter<byte> Footer { get; } = new(); 
    }

    public sealed class HtmlMain
    {
        public ArrayBufferWriter<byte> Header { get; } = new();

        public ArrayBufferWriter<byte> Article { get; } = new();
    }
}