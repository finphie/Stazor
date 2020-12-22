using System.ComponentModel.DataAnnotations;
using Stazor.Core;
using Stazor.Plugins.IO;
using Stazor.Plugins.Metadata;

namespace Stazor.Themes
{
    public sealed class StazorSettings : StazorBaseSettings
    {
        public BreadcrumbSettings Breadcrumb { get; set; } = new();

        public ReadFilesSettings ReadFiles { get; set; } = new();
    }
}