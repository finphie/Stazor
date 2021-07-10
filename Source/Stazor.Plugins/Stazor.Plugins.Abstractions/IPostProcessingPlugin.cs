﻿namespace Stazor.Plugins
{
    /// <summary>
    /// 後処理を定義するプラグイン
    /// </summary>
    public interface IPostProcessingPlugin : IPlugin
    {
        void AfterExecute(IStazorDocument[] documents);
    }
}