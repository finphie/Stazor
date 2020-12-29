using Stazor.Core;
using Stazor.Core.Helpers;

namespace Stazor.Plugins.Metadata
{
    public sealed record ViewportSettings : IStazorSettings, IValidatable
    {
        public string Content { get; init; } = "width=device-width, initial-scale=1";

        /// <inheritdoc/>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Content))
            {
                ThrowHelper.ThrowArgumentNullOrWhitespaceException(nameof(Content));
            }
        }
    }
}