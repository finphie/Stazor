using Stazor.Core;
using Stazor.Core.Helpers;

namespace Stazor.Plugins.Metadata
{
    public sealed class ViewportSettings : IValidatable
    {
        public string Content { get; set; } = "width=device-width, initial-scale=1";

        /// <inheritdoc/>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Content))
            {
                throw ThrowHelper.CreateArgumentNullOrWhitespaceException(nameof(Content));
            }
        }
    }
}