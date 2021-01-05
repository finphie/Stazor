using Stazor.Core;
using Stazor.Core.Helpers;

namespace Stazor.Plugins.Metadata
{
    public sealed record ViewportSettings : IStazorKey, IValidatable
    {
        /// <inheritdoc/>
        public string Key { get; init; } = nameof(Viewport);

        public string Content { get; init; } = "width=device-width, initial-scale=1";

        /// <inheritdoc/>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Key))
            {
                ThrowHelper.ThrowArgumentNullOrWhitespaceException(nameof(Key));
            }

            if (string.IsNullOrWhiteSpace(Content))
            {
                ThrowHelper.ThrowArgumentNullOrWhitespaceException(nameof(Content));
            }
        }
    }
}