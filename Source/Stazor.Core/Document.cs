namespace Stazor.Core
{
    /// <summary>
    /// Creates a new instance of the <see cref="IStazorDocument"/>.
    /// </summary>
    public static class Document
    {
        /// <summary>
        /// Creates a new instance of the <see cref="StazorDocument"/> class.
        /// </summary>
        /// <param name="templatePath">The relative or absolute path to the template directory.</param>
        /// <returns>The document instance.</returns>
        public static IStazorDocument GetDocument(string templatePath, IStazorContext context, IStazorMetadata metadata)
            => new StazorDocument(templatePath, context, metadata);
    }
}