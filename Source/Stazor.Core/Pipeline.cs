using System.Collections.Generic;
using Stazor.Core.Helpers;

namespace Stazor.Core
{
    /// <summary>
    /// The pipeline contains a list of plugins.
    /// </summary>
    public sealed class Pipeline
    {
        readonly List<IPlugin> _plugins;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pipeline"/> class.
        /// </summary>
        public Pipeline()
            => _plugins = new List<IPlugin>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Pipeline"/> class.
        /// </summary>
        /// <param name="plugins">The list of plugins.</param>
        public Pipeline(IEnumerable<IPlugin> plugins)
            => _plugins = new List<IPlugin>(plugins);

        /// <summary>
        /// Adds an object to the end of the pipeline.
        /// </summary>
        /// <param name="plugin">The object to be added to the end of the pipeline.</param>
        public void Add(IPlugin plugin) => _plugins.Add(plugin);

        /// <summary>
        /// Executes the job.
        /// </summary>
        /// <returns>Returns the document sequence.</returns>
        public async IAsyncEnumerable<IStazorDocument> ExecuteAsync()
        {
            var inputs = AsyncEnumerableHelpers.Empty<IStazorDocument>();

            foreach (var plugin in _plugins)
            {
                inputs = plugin.ExecuteAsync(inputs);
            }

            await foreach (var input in inputs)
            {
                yield return input;
            }
        }
    }
}