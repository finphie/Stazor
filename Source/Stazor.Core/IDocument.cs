using System.Collections.Generic;

namespace Stazor.Core
{
    /// <summary>
    /// A document consists of a set of content and metadata attributes.
    /// </summary>
    // TODO: document
    public interface IDocument
    {
        /// <summary>
        /// Gets the content associated with this document.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        Dictionary<byte[], byte[]> Content { get; }

        /// <summary>
        /// Gets the template path associated with this document.
        /// </summary>
        /// <value>
        /// The template path.
        /// </value>
        string TemplatePath { get; init; }

        /// <summary>
        /// Gets the metadata associated with this document.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        IMetadata Metadata { get; }
    }
}