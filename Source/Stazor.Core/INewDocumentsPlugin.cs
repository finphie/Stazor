﻿namespace Stazor.Core
{
    /// <summary>
    /// ドキュメント新規作成用プラグイン
    /// </summary>
    public interface INewDocumentsPlugin : IPlugin
    {
        IStazorDocument CreateDocument(string filePath);
    }
}