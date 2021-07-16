using System;
using System.Runtime.CompilerServices;
using Stazor.Core;
using Stazor.Logging;
using Stazor.Plugins;

namespace Stazor.Themes
{
    /// <summary>
    /// The pipeline contains a list of plugins.
    /// </summary>
    public abstract class Pipeline : IPipeline
    {
        readonly IStazorLogger _logger;
        readonly INewDocumentsPlugin _newDocumentsPlugin;
        readonly IEditDocumentPlugin[] _editDocumentPlugins;
        readonly IPostProcessingPlugin[] _postProcessingPlugins;

        protected INewDocumentsPlugin NewDocumentsPlugin => _newDocumentsPlugin;

        protected ReadOnlySpan<IEditDocumentPlugin> EditDocumentPlugins => _editDocumentPlugins;

        protected ReadOnlySpan<IPostProcessingPlugin> PostProcessingPlugins => _postProcessingPlugins;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pipeline"/> class.
        /// </summary>
        public Pipeline(IStazorLogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Executes the job.
        /// </summary>
        /// <returns>Returns the document sequence.</returns>
        public virtual IStazorDocument[] Execute(string[] filePaths)
        {
            var documents = Document.CreateArray(filePaths.Length);
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

        protected void Initialize(INewDocumentsPlugin newDocumentsPlugin, IEditDocumentPlugin[] editDocumentPlugins, IPostProcessingPlugin[] postProcessingPlugins)
        {
            Unsafe.AsRef(_newDocumentsPlugin) = newDocumentsPlugin;
            Unsafe.AsRef(_editDocumentPlugins) = editDocumentPlugins;
            Unsafe.AsRef(_postProcessingPlugins) = postProcessingPlugins;
        }
    }

    readonly struct DocumentCreator : Microsoft.Toolkit.HighPerformance.Helpers.IAction
    {
        readonly IStazorDocument[] _documents;
        readonly string[] _filePaths;

        readonly INewDocumentsPlugin _newDocumentsPlugin;
        readonly IEditDocumentPlugin[] _editDocumentPlugins;

        public DocumentCreator(IStazorDocument[] documents, string[] filePaths, INewDocumentsPlugin newDocumentsPlugin, IEditDocumentPlugin[] editDocumentPlugins)
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