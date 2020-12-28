using Stazor.Core;
using Stazor.Plugins.IO;
using Stazor.Plugins.Metadata;
using Stazor.Plugins.Renderer;

namespace Stazor.Themes
{
    public sealed class StazorSettings : StazorBaseSettings
    {
        public BreadcrumbSettings Breadcrumb { get; init; } = new();

        public ReadFilesSettings ReadFiles { get; init; } = new();

        public MarkdownSettings Markdown { get; init; } = new();

        public ViewportSettings Viewport { get; init; } = new();

        public StyleSheetSettings StyleSheet { get; init; } = new();

        public FaviconSettings Favicon { get; init; } = new();

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