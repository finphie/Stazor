using Stazor.Core;
using Stazor.Plugins.Metadata.Helpers;
using Utf8Utility;

namespace Stazor.Plugins.Metadata
{
    public sealed record BreadcrumbSettings : IStazorKey, IValidatable
    {
        /// <inheritdoc/>
        public Utf8String Key { get; init; }

        public Utf8String JsonLdKey { get; init; }

        public bool JsonLd { get; init; }

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