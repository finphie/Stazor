using Microsoft.Toolkit.HighPerformance.Helpers;
using Stazor.Core;
using Stazor.Logging;
using Stazor.Plugins;

namespace Stazor.Themes;

/// <summary>
/// パイプラインは、プラグインの集合体で構成されます。
/// </summary>
public abstract class Pipeline : IPipeline
{
    readonly IStazorLogger _logger;
    readonly IEditDocumentPlugin[] _editDocumentPlugins;
    readonly IPostProcessingPlugin[] _postProcessingPlugins;

    /// <summary>
    /// <see cref="Pipeline"/>クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="logger">ロガー</param>
    /// <param name="newDocumentsPlugin">ドキュメント新規作成用プラグイン</param>
    /// <param name="editDocumentPlugins">ドキュメント編集用プラグインの配列</param>
    /// <param name="postProcessingPlugins">後処理を行うプラグインの配列</param>
    public Pipeline(IStazorLogger logger, INewDocumentsPlugin newDocumentsPlugin, IEditDocumentPlugin[] editDocumentPlugins, IPostProcessingPlugin[] postProcessingPlugins)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        NewDocumentsPlugin = newDocumentsPlugin ?? throw new ArgumentNullException(nameof(newDocumentsPlugin));
        _editDocumentPlugins = editDocumentPlugins ?? throw new ArgumentNullException(nameof(editDocumentPlugins));
        _postProcessingPlugins = postProcessingPlugins ?? throw new ArgumentNullException(nameof(postProcessingPlugins));
    }

    /// <summary>
    /// ドキュメント新規作成用プラグイン
    /// </summary>
    protected INewDocumentsPlugin NewDocumentsPlugin { get; }

    /// <summary>
    /// ドキュメント編集用プラグインのリスト
    /// </summary>
    protected ReadOnlySpan<IEditDocumentPlugin> EditDocumentPlugins => _editDocumentPlugins;

    /// <summary>
    /// 後処理を行うプラグインのリスト
    /// </summary>
    protected ReadOnlySpan<IPostProcessingPlugin> PostProcessingPlugins => _postProcessingPlugins;

    /// <inheritdoc/>
    public virtual IStazorDocument[] Execute(string[] filePaths)
    {
        ArgumentNullException.ThrowIfNull(filePaths);
        _logger.Debug("Start");

        var documents = Document.CreateArray(filePaths.Length);
        var sw = System.Diagnostics.Stopwatch.StartNew();

        ParallelHelper.For(0, filePaths.Length, new DocumentCreator(documents, filePaths, NewDocumentsPlugin, _editDocumentPlugins));

        sw.Stop();
        Console.WriteLine(sw.Elapsed);

        foreach (var plugin in _postProcessingPlugins)
        {
            plugin.AfterExecute(documents);
        }

        return documents;
    }
}