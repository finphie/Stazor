using Stazor.Plugins.Metadata.Helpers;
using Utf8Utility;

namespace Stazor.Plugins.Metadata
{
    public sealed record BreadcrumbSettings : ISettingsKey
    {
        public static string Key => nameof(Breadcrumb);

        public string ContextKey { get; set; }

        public Utf8String JsonLdKey = (Utf8String)"JsonLd";

        public bool JsonLd { get; set; }

        /// <inheritdoc/>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Key))
            {
                ThrowHelper.ThrowArgumentNullException(nameof(Key));
            }
        }
    }
}