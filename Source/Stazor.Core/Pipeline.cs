using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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
        public async ValueTask<IDocumentList> ExecuteAsync(string[] filePaths)
        {
            var documents = new StazorDocument[filePaths.Length];
            var list = new DocumentList();
            var sw = System.Diagnostics.Stopwatch.StartNew();

            //foreach (var filePath in filePaths)
            //{
            //    var document = await _newDocumentsPlugin.CreateDocumentAsync(filePath).ConfigureAwait(false);

            //    foreach (var plugin in _editDocumentPlugins)
            //    {
            //        await plugin.ExecuteAsync(document).ConfigureAwait(false);
            //    }

            //    list.Add(document);
            //}

            //Microsoft.Toolkit.HighPerformance.Helpers.ParallelHelper.ForEach<IStazorDocument, A>(ds, new A(ps));
            Microsoft.Toolkit.HighPerformance.Helpers.ParallelHelper.For(0, filePaths.Length, new Assigner(documents, filePaths, _newDocumentsPlugin, _editDocumentPlugins));

            sw.Stop();
            System.Console.WriteLine(sw.Elapsed);

            //foreach (var plugin in _postProcessingPlugins)
            //{
            //    await plugin.AfterExecuteAsync(documents).ConfigureAwait(false);
            //}

            foreach (var x in documents)
            {
                list.Add(x);
            }

            return list;
        }
    }

    public readonly struct Assigner : Microsoft.Toolkit.HighPerformance.Helpers.IAction
    {
        readonly IStazorDocument[] _documents;
        readonly string[] _filePaths;

        readonly INewDocumentsPlugin _newDocumentsPlugin;
        readonly List<IEditDocumentPlugin> _editDocumentPlugins;

        public Assigner(IStazorDocument[] documents, string[] filePaths, INewDocumentsPlugin newDocumentsPlugin, List<IEditDocumentPlugin> editDocumentPlugins)
        {
            _documents = documents;
            _filePaths = filePaths;

            _newDocumentsPlugin = newDocumentsPlugin;
            _editDocumentPlugins = editDocumentPlugins;
        }

        public void Invoke(int i)
        {
            var documentTask = _newDocumentsPlugin.CreateDocumentAsync(_filePaths[i]);
            if (!documentTask.IsCompleted)
            {
                throw new InvalidOperationException();
            }

            var document = documentTask.GetAwaiter().GetResult();

            foreach (var plugin in _editDocumentPlugins)
            {
                var pluginTask = plugin.ExecuteAsync(document);
                if (!pluginTask.IsCompleted)
                {
                    throw new InvalidOperationException();
                }

                pluginTask.GetAwaiter().GetResult();
            }

            _documents[i] = document;
        }
    }
}