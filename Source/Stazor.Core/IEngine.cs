using System.Buffers;
using System.Threading.Tasks;
using Stazor.Core;

namespace Stazor.Engine
{
    public interface IEngine
    {
        string Name { get; }

        string Description { get; }

        ValueTask ExecuteAsync(IBufferWriter<byte> bufferWriter, IDocument document);
    }
}