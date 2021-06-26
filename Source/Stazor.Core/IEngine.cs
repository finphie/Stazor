using System.Buffers;
using System.Threading.Tasks;
using Stazor.Core;

namespace Stazor.Engine
{
    /// <summary>
    /// Represents a template engine.
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// Gets the name associated with the engine.
        /// </summary>
        /// <value>
        /// The engine name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the description associated with the engine.
        /// </summary>
        /// <value>
        /// The engine description.
        /// </value>
        string Description { get; }

        /// <summary>
        /// Executes the job.
        /// </summary>
        /// <param name="bufferWriter">The target writer.</param>
        /// <param name="document">The document.</param>
        /// <returns>A <see cref="ValueTask"/> for the asynchronous operation.</returns>
        ValueTask ExecuteAsync(IBufferWriter<byte> bufferWriter, IStazorDocument document);
    }
}