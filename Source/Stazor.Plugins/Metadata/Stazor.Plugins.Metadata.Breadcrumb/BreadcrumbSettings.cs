using Stazor.Core;
using Stazor.Core.Helpers;

namespace Stazor.Plugins.Metadata
{
    public sealed class BreadcrumbSettings : IValidatable
    {
        public bool JsonLd { get; set; }

        // TODO: 不要なのでは
        public string SiteUrl { get; set; }

        /// <inheritdoc/>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(SiteUrl))
            {
                ThrowHelper.CreateArgumentNullOrWhitespaceException(nameof(SiteUrl));
            }
        }
    }
}