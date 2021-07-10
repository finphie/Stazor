using Stazor.Logging;
using Stazor.Plugins;
using Stazor.Plugins.Contents;
using Stazor.Plugins.IO;
using Stazor.Plugins.Metadata;

namespace Stazor.Themes
{
    sealed class SimpleBlogPipeline : Pipeline
    {
        public SimpleBlogPipeline(IStazorLogger logger, IPluginProvider pluginProvider)
            : base(logger)
        {
            var newDocumentsPlugin = pluginProvider.GetPlugin<ReadMarkdownFiles>();
            var editDocumentPlugins = new[]
            {
                pluginProvider.GetPlugin<Breadcrumb>()
            };
            var postProcessingPlugins = new[]
            {
                pluginProvider.GetPlugin<Sort>()
            };

            Initialize(newDocumentsPlugin, editDocumentPlugins, postProcessingPlugins);
        }
    }
}