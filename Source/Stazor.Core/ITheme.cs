using System.Threading.Tasks;
using Stazor.Engine;

namespace Stazor.Core
{
    /// <summary>
    /// Theme contains a set of content such as engine, pipeline, HTML files and CSS files.
    /// </summary>
    public interface ITheme
    {
        /// <summary>
        /// Gets the engine instance associated with the theme.
        /// </summary>
        /// <value>
        /// The engine instance.
        /// </value>
        IEngine Engine { get; }

        /// <summary>
        /// Gets the pipeline associated with the theme.
        /// </summary>
        /// <value>
        /// The pipeline.
        /// </value>
        Pipeline Pipeline { get; }

        /// <summary>
        /// Executes the job.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> for the asynchronous operation.</returns>
        ValueTask ExecuteAsync();
    }
}