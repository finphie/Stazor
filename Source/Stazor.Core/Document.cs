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
        public static IStazorDocument Create(string templatePath)
            => new StazorDocument(templatePath, Context.Create(), Metadata.Create());
    }
}