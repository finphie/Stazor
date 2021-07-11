namespace Stazor.Plugins
{
    public interface IPluginResolver
    {
        T GetPlugin<T>() where T : IPlugin;
    }
}