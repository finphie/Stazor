﻿using System.Collections.Generic;
using Stazor.Core.Helpers;

namespace Stazor.Core
{
    public sealed class Pipeline
    {
        readonly List<IPlugin> _plugins;

        public Pipeline()
            => _plugins = new List<IPlugin>();

        public Pipeline(IEnumerable<IPlugin> plugins)
            => _plugins = new List<IPlugin>(plugins);

        public void Add(IPlugin plugin)
        {
            _plugins.Add(plugin);
        }

        public async IAsyncEnumerable<IDocument> ExecuteAsync()
        {
            var inputs = AsyncEnumerableHelpers.Empty<IDocument>();

            foreach (var plugin in _plugins)
            {
                inputs = plugin.ExecuteAsync(inputs);
            }

            await foreach (var input in inputs)
            {
                yield return input;
            }
        }
    }
}