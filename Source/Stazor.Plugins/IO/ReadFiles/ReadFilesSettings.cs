namespace Stazor.Plugins.IO
{
    public sealed class ReadFilesSettings
    {
        /// <summary>
        /// The relative or absolute path to the directory to search.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The relative or absolute path to the template directory.
        /// </summary>
        public string TemplatePath { get; set; }

        /// <summary>
        /// The search string to match against the names of files in path.
        /// </summary>
        public string SearchPattern { get; set; }
    }
}