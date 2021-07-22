using Microsoft.Toolkit.HighPerformance.Helpers;
using Stazor.Plugins;

namespace Stazor.Themes
{
    /// <summary>
    /// ドキュメントを並列で作成するための構造体です。
    /// </summary>
    readonly struct DocumentCreator : IAction
    {
        readonly IStazorDocument[] _documents;
        readonly string[] _filePaths;

        readonly INewDocumentsPlugin _newDocumentsPlugin;
        readonly IEditDocumentPlugin[] _editDocumentPlugins;

        /// <summary>
        /// <see cref="DocumentCreator"/>構造体の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="documents">ドキュメントの配列</param>
        /// <param name="filePaths">ファイルパスの配列</param>
        /// <param name="newDocumentsPlugin">ドキュメント新規作成用プラグイン</param>
        /// <param name="editDocumentPlugins">ドキュメント編集用プラグインの配列</param>
        public DocumentCreator(IStazorDocument[] documents, string[] filePaths, INewDocumentsPlugin newDocumentsPlugin, IEditDocumentPlugin[] editDocumentPlugins)
        {
            _documents = documents;
            _filePaths = filePaths;

            _newDocumentsPlugin = newDocumentsPlugin;
            _editDocumentPlugins = editDocumentPlugins;
        }

        /// <inheritdoc/>
        public void Invoke(int i)
        {
            var document = _newDocumentsPlugin.CreateDocument(_filePaths[i]);

            foreach (var plugin in _editDocumentPlugins)
            {
                plugin.Execute(document);
            }

            _documents[i] = document;
        }
    }
}