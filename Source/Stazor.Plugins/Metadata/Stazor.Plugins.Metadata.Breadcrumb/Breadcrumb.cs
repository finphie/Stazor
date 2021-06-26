﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Cysharp.Text;
using Stazor.Core;
using Utf8Json;
using Utf8Json.Resolvers;
using Utf8Utility;

namespace Stazor.Plugins.Metadata
{
    /// <summary>
    /// Create a Breadcrumb Navigation.
    /// </summary>
    public sealed class Breadcrumb : IPlugin
    {
        readonly IStazorLogger _logger;
        readonly BreadcrumbSettings _settings;

        public Breadcrumb(IStazorLogger logger, BreadcrumbSettings settings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<IStazorDocument> ExecuteAsync(IAsyncEnumerable<IStazorDocument> inputs)
        {
            _logger.Information("Start");

            using var builder = ZString.CreateUtf8StringBuilder(true);

            // TODO: "ホーム"文字列を可変
            builder.Append("<nav><ol class=\"breadcrumbs\"><li><a href=\"/\">ホーム</a><li><a href=\"");

            var length = builder.Length;

#pragma warning disable CA1508 // 使用されない条件付きコードを回避する
            await foreach (var input in inputs.ConfigureAwait(false))
#pragma warning restore CA1508 // 使用されない条件付きコードを回避する
            {
                builder.Append(input.Metadata.Category);
                builder.Append("\">");
                builder.Append(input.Metadata.Category);
                builder.Append("</a>");
                builder.Append("<li>");
                builder.Append(input.Metadata.Title);

                builder.Append("</ol>");
                builder.Append("</nav>");

                input.Context.Add(_settings.Key, new Utf8String(builder.AsSpan().ToArray()));

                if (_settings.JsonLd)
                {
                    builder.Advance(-length);

                    // TODO: JSON-LD 高速化
                    var json = new JsonLd
                    {
                        Context = "https://schema.org",
                        Type = "BreadcrumbList",
                        Items = new JsonLd.ItemListElement[2]
                    };

                    for (var i = 0; i < json.Items.Length; i++)
                    {
                        json.Items[i] = new();
                        json.Items[i].Type = "ListItem";
                        json.Items[i].Position = i + 1;
                    }

                    json.Items[0].Name = "ホーム";
                    json.Items[1].Name = input.Metadata.Category!;
                    json.Items[0].Item = "/";
                    json.Items[1].Item = "/" + input.Metadata.Category;

                    var jsonLd = JsonSerializer.Serialize(json, StandardResolver.AllowPrivateExcludeNullSnakeCase);
                    input.Context.Add(_settings.JsonLdKey, new Utf8String(jsonLd));
                }

                yield return input;
            }

            _logger.Information("End");
        }

        sealed class JsonLd
        {
            [DataMember(Name = "@context")]
            public string? Context { get; set; }

            [DataMember(Name = "@type")]
            public string? Type { get; set; }

            [DataMember(Name = "itemListElement")]
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