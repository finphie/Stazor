using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Stazor.Core;
using Utf8Json;
using Utf8Json.Resolvers;

namespace Stazor.Plugins.Metadata
{
    public sealed class Breadcrumb : IPlugin
    {
        static readonly byte[] StartNav = Encoding.UTF8.GetBytes("<nav>");
        static readonly byte[] EndNav = Encoding.UTF8.GetBytes("</nav>");
        static readonly byte[] StartOl = Encoding.UTF8.GetBytes("<ol class=\"breadcrumbs\">");
        static readonly byte[] EndOl = Encoding.UTF8.GetBytes("</ol>");

        static readonly byte[] Li1 = Encoding.UTF8.GetBytes("<li><a href=\"/\">ホーム</a>");
        static readonly byte[] StartLi2 = Encoding.UTF8.GetBytes("<li><a href=\"");


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
            
            await foreach (var input in inputs.ConfigureAwait(false))
            {
                input.Content.Body.Main.Header.Write(StartNav);
                input.Content.Body.Main.Header.Write(StartOl);
                input.Content.Body.Main.Header.Write(Li1);
                input.Content.Body.Main.Header.Write(StartLi2);
                input.Content.Body.Main.Header.Write(Encoding.UTF8.GetBytes(input.Metadata.Category!));
                input.Content.Body.Main.Header.Write(Encoding.UTF8.GetBytes("\">"));
                input.Content.Body.Main.Header.Write(Encoding.UTF8.GetBytes(input.Metadata.Category!));
                input.Content.Body.Main.Header.Write(Encoding.UTF8.GetBytes("</a>"));
                input.Content.Body.Main.Header.Write(Encoding.UTF8.GetBytes("<li>"));
                input.Content.Body.Main.Header.Write(Encoding.UTF8.GetBytes(input.Metadata.Title!));

                input.Content.Body.Main.Header.Write(EndOl);
                input.Content.Body.Main.Header.Write(EndNav);

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