namespace Stazor.Plugins
{
    public interface IPluginProvider
    {
        T GetPlugin<T>() where T : IPlugin;
    }
}