using Stazor.Plugins.Metadata.Helpers;
using Utf8Utility;

namespace Stazor.Plugins.Metadata
{
    public sealed record BreadcrumbSettings
    {
        /// <inheritdoc/>
        public Utf8String Key = (Utf8String)nameof(Breadcrumb);

        public Utf8String JsonLdKey = (Utf8String)"JsonLd";

        public bool JsonLd { get; set; }

        /// <inheritdoc/>
        public void Validate()
        {
            if (Key == Utf8String.Empty)
            {
                ThrowHelper.ThrowArgumentNullException(nameof(Key));
            }
        }
    }
}