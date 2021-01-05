using Stazor.Core;
using Stazor.Plugins.IO;
using Stazor.Plugins.Metadata;

namespace Stazor.Themes
{
    public sealed record StazorSettings : StazorBaseSettings
    {
        public BreadcrumbSettings Breadcrumb { get; init; } = new();

        public ReadMarkdownFilesSettings Markdown { get; init; } = new();

        /// <inheritdoc/>
        public override void Validate()
        {
            base.Validate();
            Breadcrumb.Validate();
            Markdown.Validate();
        }
    }
}