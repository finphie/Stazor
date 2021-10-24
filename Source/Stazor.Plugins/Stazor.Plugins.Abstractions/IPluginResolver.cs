namespace Stazor.Plugins;

/// <summary>
/// プラグインのオブジェクトを取得します。
/// </summary>
public interface IPluginResolver
{
    /// <summary>
    /// プラグインのオブジェクトを取得します。
    /// </summary>
    /// <typeparam name="T">対象のプラグイン</typeparam>
    /// <returns>プラグインのオブジェクト</returns>
    T GetPlugin<T>()
        where T : IPlugin;
}
