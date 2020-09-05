using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Text;
using Stazor.Core;
using Utf8Json;
using Utf8Json.Resolvers;

namespace Stazor.Plugins.Metadata
{
    public sealed class Breadcrumb : IPlugin
    {
        public async IAsyncEnumerable<IDocument> ExecuteAsync(IAsyncEnumerable<IDocument> inputs)
        {
            var json = new JsonLd();
            json.Context = "https://schema.org";
            json.Type = "BreadcrumbList";

            json.Items = new JsonLd.ItemListElement[2];

            for (var i = 0; i < json.Items.Length; i++)
            {
                json.Items[i] = new();
                json.Items[i].Type = "ListItem";
                json.Items[i].Position = i + 1;
                json.Items[0].Item = "https://example.com";
            }

            json.Items[0].Name = "ホーム";

            using var builder = ZString.CreateUtf8StringBuilder(true);
            builder.Append("<nav><ol class=\"breadcrumbs\"><li><a href=\"/\">ホーム</a><li><a href=\"");

            var length = builder.Length;

            await foreach (var input in inputs.ConfigureAwait(false))
            {       
                builder.Append(input.Metadata.Category);
                builder.Append("\">");
                builder.Append(input.Metadata.Category);
                builder.Append("</a>");
                builder.Append("<li>");
                builder.Append(input.Metadata.Title);

                builder.Append("</ol>");
                builder.Append("</nav>");

                input.Content.Add(nameof(Breadcrumb), builder.AsSpan().ToArray());
                builder.Advance(-length);

                json.Items[1].Name = input.Metadata.Category!;
                json.Items[1].Item = "https://example.com/";
                // TODO
                var a = JsonSerializer.Serialize(json, StandardResolver.AllowPrivateExcludeNullSnakeCase);

                yield return input;
            }
        }

        sealed class JsonLd
        {
            [DataMember(Name = "@context")]
            public string? Context { get; set; }

            [DataMember(Name = "@type")]
            public string? Type { get; set; }

            public ItemListElement[]? Items { get; set; }

            public sealed class ItemListElement
            {
                [DataMember(Name = "@type")]
                public string? Type { get; set; }

                public int Position { get; set; }

                public string? Name { get; set; }

                public string? Item { get; set; }
            }
        }
    }
}