using System;
using Microsoft.Extensions.DependencyInjection;

namespace Stazor.Plugins
{
    public sealed class PluginResolver : IPluginResolver
    {
        readonly IServiceProvider _service;

        public PluginResolver(IServiceProvider service)
            => _service = service;

        /// <inheritdoc/>
        public T GetPlugin<T>()
            where T : IPlugin
            => _service.GetRequiredService<T>();
    }
}