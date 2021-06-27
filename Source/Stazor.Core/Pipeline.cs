using System.Collections.Generic;

namespace Stazor.Core
{
    /// <summary>
    /// The pipeline contains a list of plugins.
    /// </summary>
    public sealed class Pipeline
    {
        readonly INewDocumentsPlugin _newDocumentsPlugin;
        readonly List<IEditDocumentPlugin> _editDocumentPlugins;
        readonly List<IPostProcessingPlugin> _postProcessingPlugins;

        readonly IDocumentList _documents;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pipeline"/> class.
        /// </summary>
        public Pipeline(INewDocumentsPlugin newDocumentsPlugin)
        {
            _newDocumentsPlugin = newDocumentsPlugin;
            _editDocumentPlugins = new();
            _postProcessingPlugins = new();
        }

        /// <summary>
        /// Adds an object to the end of the pipeline.
        /// </summary>
        /// <param name="plugin">The object to be added to the end of the pipeline.</param>
        public void Add(IEditDocumentPlugin plugin) => _editDocumentPlugins.Add(plugin);

        /// <summary>
        /// Executes the job.
        /// </summary>
        /// <returns>Returns the document sequence.</returns>
        public async IAsyncEnumerable<IStazorDocument> ExecuteAsync()
        {
            var documents = await _newDocumentsPlugin.CreateDocumentsAsync().ConfigureAwait(false);

            // TODO: 並列化
            for (var i = 0; i < documents.Length; i++)
            {
                foreach (var plugin in _editDocumentPlugins)
                {
                    await plugin.ExecuteAsync(documents[i]).ConfigureAwait(false);
                }
            }

            foreach (var plugin in _postProcessingPlugins)
            {
                await plugin.AfterExecuteAsync(documents).ConfigureAwait(false);
            }
        }
    }
}