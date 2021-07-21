using Stazor.Logging;
using Stazor.Plugins;
using Stazor.Plugins.Contents;
using Stazor.Plugins.IO;
using Stazor.Plugins.Metadata;

namespace Stazor.Themes
{
    /// <summary>
    /// <see cref="SimpleBlog"/>のパイプラインです。
    /// </summary>
    sealed class SimpleBlogPipeline : Pipeline
    {
        /// <summary>
        /// <see cref="SimpleBlogPipeline"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="logger">ロガー</param>
        /// <param name="pluginResolver">プラグインリゾルバ</param>
        public SimpleBlogPipeline(IStazorLogger<SimpleBlogPipeline> logger, IPluginResolver pluginResolver)
            : base(logger)
        {
            var newDocumentsPlugin = pluginResolver.GetPlugin<ReadMarkdownFiles>();
            var editDocumentPlugins = new[]
            {
                pluginResolver.GetPlugin<Breadcrumb>()
            };
            var postProcessingPlugins = new[]
            {
                pluginResolver.GetPlugin<Sort>()
            };

            Initialize(newDocumentsPlugin, editDocumentPlugins, postProcessingPlugins);
        }
    }
}