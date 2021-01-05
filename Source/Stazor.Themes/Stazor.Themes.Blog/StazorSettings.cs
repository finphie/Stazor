using Stazor.Core;
using Stazor.Plugins.IO;
using Stazor.Plugins.Metadata;
using Stazor.Plugins.Renderer;

namespace Stazor.Themes
{
    public sealed record StazorSettings : StazorBaseSettings
    {
        public BreadcrumbSettings Breadcrumb { get; init; } = new();

        public ReadFilesSettings ReadFiles { get; init; } = new();

        public MarkdownSettings Markdown { get; init; } = new();

        /// <inheritdoc/>
        public override void Validate()
        {
            base.Validate();
            Breadcrumb.Validate();
            ReadFiles.Validate();
            Markdown.Validate();
        }
    }
}