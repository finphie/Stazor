using Stazor.Core;
using Stazor.Plugins.IO;
using Stazor.Plugins.Metadata;
using Stazor.Plugins.Renderer;

namespace Stazor.Themes
{
    public sealed class StazorSettings : StazorBaseSettings
    {
        public BreadcrumbSettings Breadcrumb { get; set; } = new();

        public ReadFilesSettings ReadFiles { get; set; } = new();

        public MarkdownSettings Markdown { get; set; } = new();

        public ViewportSettings Viewport { get; set; } = new();

        public StyleSheetSettings StyleSheet { get; set; } = new();

        public FaviconSettings Favicon { get; set; } = new();

        /// <inheritdoc/>
        public override void Validate()
        {
            base.Validate();
            Breadcrumb.Validate();
            ReadFiles.Validate();
            Markdown.Validate();
            Viewport.Validate();
            StyleSheet.Validate();
            Favicon.Validate();
        }
    }
}