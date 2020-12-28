using Stazor.Core;
using Stazor.Core.Helpers;

namespace Stazor.Plugins.Metadata
{
    public sealed class StyleSheetSettings : IValidatable
    {
        /// <summary>
        /// The CSS file url.
        /// </summary>
        public string Href { get; set; }

        /// <inheritdoc/>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Href))
            {
                throw ThrowHelper.CreateArgumentNullOrWhitespaceException(nameof(Href));
            }
        }
    }
}