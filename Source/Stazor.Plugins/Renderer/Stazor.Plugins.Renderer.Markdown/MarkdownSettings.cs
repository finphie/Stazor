using Stazor.Core;
using Stazor.Core.Helpers;

namespace Stazor.Plugins.Renderer
{
    public sealed class MarkdownSettings : IValidatable
    {
        public string InputKey { get; set; }

        /// <inheritdoc/>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(InputKey))
            {
                throw ThrowHelper.CreateArgumentNullOrWhitespaceException(nameof(InputKey));
            }
        }
    }
}