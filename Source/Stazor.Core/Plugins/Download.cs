using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Stazor.Core.Plugins
{
    public sealed class Download : IPlugin
    {
        readonly HttpClient _client;
        readonly HttpRequestMessage _request;

        public Download(HttpClient client, string requestUri)
            : this(client, new Uri(requestUri))
        {
        }

        public Download(HttpClient client, Uri requestUri)
            : this(client, new HttpRequestMessage(HttpMethod.Get, requestUri))
        {
        }

        public Download(HttpClient client, HttpRequestMessage request)
            => (_client, _request) = (client, request);

        public async IAsyncEnumerable<IDocument> ExecuteAsync(IAsyncEnumerable<IDocument> inputs)
        {
            await foreach (var input in inputs)
            {
                yield return input;
            }

            var response = await _client.SendAsync(_request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            yield return DocumentFactory.GetDocument(result);
        }
    }
}