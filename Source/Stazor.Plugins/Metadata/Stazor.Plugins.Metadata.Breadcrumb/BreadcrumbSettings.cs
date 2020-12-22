using System.ComponentModel.DataAnnotations;

namespace Stazor.Plugins.Metadata
{
    public sealed class BreadcrumbSettings
    {
        [Required]
        public bool JsonLd { get; set; }

        public string SiteUrl { get; set; }
    }
}