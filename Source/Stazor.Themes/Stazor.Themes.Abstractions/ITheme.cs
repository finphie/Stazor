using System.Threading.Tasks;

namespace Stazor.Themes
{
    /// <summary>
    /// Theme contains a set of content such as engine, pipeline, HTML files and CSS files.
    /// </summary>
    public interface ITheme
    {
        /// <summary>
        /// Executes the job.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> for the asynchronous operation.</returns>
        ValueTask ExecuteAsync();
    }
}