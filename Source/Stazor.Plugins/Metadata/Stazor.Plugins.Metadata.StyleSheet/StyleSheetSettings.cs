﻿using System.Diagnostics.CodeAnalysis;
using Stazor.Core;
using Stazor.Core.Helpers;

namespace Stazor.Plugins.Metadata
{
    public sealed class StyleSheetSettings : IStazorSettings, IValidatable
    {
        /// <summary>
        /// The CSS file url.
        /// </summary>
        [DisallowNull]
        public string? Href { get; init; }

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