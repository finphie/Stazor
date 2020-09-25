using System;
using System.Collections.Generic;

namespace Stazor.Core
{
    /// <summary>
    /// The document metadata.
    /// </summary>
    public interface IMetadata
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string? Title { get; set; }

        /// <summary>
        /// Gets or sets the published date.
        /// </summary>
        /// <value>
        /// The published date.
        /// </value>
        DateTimeOffset PublishedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        DateTimeOffset? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        /// <value>
        /// The category name.
        /// </value>
        string? Category { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        IReadOnlySet<string> Tags { get; set; }
    }
}