namespace Stazor.Plugins
{
    public interface IPluginCollection
    {
        INewDocumentsPlugin GetNewDocumentsPlugin();

        IEditDocumentPlugin[] GetEditDocumentPlugins();

        IPostProcessingPlugin[] GetPostProcessingPlugins();
    }
}