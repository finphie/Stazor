using Stazor.Core;
using Stazor.Plugins.IO;
using Stazor.Plugins.Metadata;
using Utf8Utility;

namespace Stazor.Themes
{
    public sealed record StazorSettings : StazorBaseSettings
    {
        public BreadcrumbSettings Breadcrumb { get; set; } = new();

        public ReadMarkdownFilesSettings Markdown { get; set; } = new();

        /// <inheritdoc/>
        public override void Validate()
        {
            base.Validate();
            Breadcrumb.Validate();
            Markdown.Validate();
        }
    }
}