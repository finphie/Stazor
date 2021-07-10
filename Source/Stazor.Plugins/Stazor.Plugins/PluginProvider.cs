using System;
using Microsoft.Extensions.DependencyInjection;

namespace Stazor.Plugins
{
    public sealed class PluginProvider : IPluginProvider
    {
        readonly IServiceProvider _service;

        public PluginProvider(IServiceProvider service)
            => _service = service;

        /// <inheritdoc/>
        public T GetPlugin<T>()
            where T : IPlugin
            => _service.GetRequiredService<T>();
    }
}