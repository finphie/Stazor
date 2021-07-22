using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Stazor.Core;
using Stazor.Logging;
using Stazor.Plugins;

namespace Stazor.Themes
{
    /// <summary>
    /// パイプラインは、プラグインの集合体で構成されます。
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
        /// <see cref="Pipeline"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="logger">ロガー</param>
        public Pipeline(IStazorLogger logger) => _logger = logger;

        /// <inheritdoc/>
        public virtual IStazorDocument[] Execute(string[] filePaths)
        {
            var documents = Document.CreateArray(filePaths.Length);
            var sw = System.Diagnostics.Stopwatch.StartNew();

            Microsoft.Toolkit.HighPerformance.Helpers.ParallelHelper.For(0, filePaths.Length, new DocumentCreator(documents, filePaths, _newDocumentsPlugin, _editDocumentPlugins));

            sw.Stop();
            Console.WriteLine(sw.Elapsed);

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
}