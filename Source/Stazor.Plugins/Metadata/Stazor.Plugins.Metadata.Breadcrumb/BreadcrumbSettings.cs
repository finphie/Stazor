using Stazor.Core;

namespace Stazor.Plugins.Metadata
{
    public sealed record BreadcrumbSettings : IStazorSettings, IValidatable
    {
        public bool JsonLd { get; init; }

        /// <inheritdoc/>
        public void Validate()
        {
        }
    }
}