using System.Collections.Generic;
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
        public async ValueTask<IDocumentList> ExecuteAsync()
        {
            //Microsoft.Toolkit.HighPerformance.Helpers.ParallelHelper.ForEach<IStazorDocument, A>(new IStazorDocument[10]);
            //Microsoft.Toolkit.HighPerformance.Helpers.ParallelHelper.ForEach(new IStazorDocument[10], new A());
            var documents = await _newDocumentsPlugin.CreateDocumentsAsync().ConfigureAwait(false);
            var ds = documents.ToArray();
            var ps = _editDocumentPlugins.ToArray();

            var sw = System.Diagnostics.Stopwatch.StartNew();

            // TODO: 並列化
            //for (var i = 0; i < ds.Length; i++)
            //{
            //    foreach (var plugin in _editDocumentPlugins)
            //    {
            //        await plugin.ExecuteAsync(ds[i]).ConfigureAwait(false);
            //    }
            //}

            Microsoft.Toolkit.HighPerformance.Helpers.ParallelHelper.ForEach<IStazorDocument, A>(ds, new A(ps));
            Microsoft.Toolkit.HighPerformance.Helpers.ParallelHelper.For(0, )

            sw.Stop();
            System.Console.WriteLine(sw.Elapsed);

            //foreach (var plugin in _postProcessingPlugins)
            //{
            //    await plugin.AfterExecuteAsync(documents).ConfigureAwait(false);
            //}

            var a = new DocumentList();
            foreach (var b in ds)
            {
                a.Add(b);
            }

            return a;
        }
    }



    public readonly struct A : Microsoft.Toolkit.HighPerformance.Helpers.IRefAction<IStazorDocument>
    {
        readonly IEditDocumentPlugin[] _editDocumentPlugins;

        public A(IEditDocumentPlugin[] editDocumentPlugins) => _editDocumentPlugins = editDocumentPlugins;

        public void Invoke(ref IStazorDocument item)
        {
            foreach (var plugin in _editDocumentPlugins)
            {
                var x = plugin.ExecuteAsync(item);

                if (x.IsCompleted)
                {
                    x.GetAwaiter().GetResult();
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }
    }
}