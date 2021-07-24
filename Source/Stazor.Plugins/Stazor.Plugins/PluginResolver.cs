using System;
using Microsoft.Extensions.DependencyInjection;

namespace Stazor.Plugins
{
    /// <summary>
    /// プラグインのオブジェクトを取得するクラスです。
    /// </summary>
    public sealed class PluginResolver : IPluginResolver
    {
        readonly IServiceProvider _provider;

        /// <summary>
        /// <see cref="PluginResolver"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="provider">プラグインのオブジェクトを取得するために必要な<see cref="IServiceProvider"/></param>
        public PluginResolver(IServiceProvider provider)
            => _provider = provider;

        /// <inheritdoc/>
        public T GetPlugin<T>()
            where T : IPlugin
            => _provider.GetRequiredService<T>();
    }
}