namespace Stazor.Core
{
    /// <summary>
    /// Creates a new instance of the <see cref="IDocument"/>.
    /// </summary>
    public static class DocumentFactory
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Document"/> class.
        /// </summary>
        /// <param name="templatePath">The relative or absolute path to the template directory.</param>
        /// <returns>The document instance.</returns>
        public static IDocument GetDocument(string templatePath)
            => new Document { TemplatePath = templatePath };
    }
}