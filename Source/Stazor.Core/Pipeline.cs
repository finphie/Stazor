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
        public IStazorDocument[] Execute(string[] filePaths)
        {
            var documents = new StazorDocument[filePaths.Length];
            var sw = System.Diagnostics.Stopwatch.StartNew();

            //foreach (var filePath in filePaths)
            //{
            //    var document = _newDocumentsPlugin.CreateDocument(filePath);

            //    foreach (var plugin in _editDocumentPlugins)
            //    {
            //        plugin.Execute(document);
            //    }

            //    list.Add(document);
            //}

            Microsoft.Toolkit.HighPerformance.Helpers.ParallelHelper.For(0, filePaths.Length, new DocumentCreator(documents, filePaths, _newDocumentsPlugin, _editDocumentPlugins));

            sw.Stop();
            System.Console.WriteLine(sw.Elapsed);

            foreach (var plugin in _postProcessingPlugins)
            {
                plugin.AfterExecute(documents);
            }

            return documents;
        }
    }

    readonly struct DocumentCreator : Microsoft.Toolkit.HighPerformance.Helpers.IAction
    {
        readonly IStazorDocument[] _documents;
        readonly string[] _filePaths;

        readonly INewDocumentsPlugin _newDocumentsPlugin;
        readonly List<IEditDocumentPlugin> _editDocumentPlugins;

        public DocumentCreator(IStazorDocument[] documents, string[] filePaths, INewDocumentsPlugin newDocumentsPlugin, List<IEditDocumentPlugin> editDocumentPlugins)
        {
            _documents = documents;
            _filePaths = filePaths;

            _newDocumentsPlugin = newDocumentsPlugin;
            _editDocumentPlugins = editDocumentPlugins;
        }

        public void Invoke(int i)
        {
            var document = _newDocumentsPlugin.CreateDocument(_filePaths[i]);

            foreach (var plugin in _editDocumentPlugins)
            {
                plugin.Execute(document);
            }

            _documents[i] = document;
        }
    }
}