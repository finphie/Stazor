using System.Diagnostics.CodeAnalysis;
using Stazor.Core;
using Stazor.Core.Helpers;

namespace Stazor.Plugins.Metadata
{
    public sealed class BreadcrumbSettings : IStazorSettings, IValidatable
    {
        public bool JsonLd { get; init; }

        // TODO: 不要なのでは
        [DisallowNull]
        public string? SiteUrl { get; init; }

        /// <inheritdoc/>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(SiteUrl))
            {
                ThrowHelper.ThrowArgumentNullOrWhitespaceException(nameof(SiteUrl));
            }
        }
    }
}