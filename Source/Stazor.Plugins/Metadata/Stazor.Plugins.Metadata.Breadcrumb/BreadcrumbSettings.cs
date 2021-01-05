using Stazor.Core;
using Stazor.Core.Helpers;

namespace Stazor.Plugins.Metadata
{
    public sealed record BreadcrumbSettings : IStazorKey, IValidatable
    {
        /// <inheritdoc/>
        public string Key { get; init; } = nameof(Breadcrumb);

        public bool JsonLd { get; init; }

        /// <inheritdoc/>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Key))
            {
                ThrowHelper.ThrowArgumentNullOrWhitespaceException(nameof(Key));
            }
        }
    }
}