namespace Stazor.Core
{
    /// <summary>
    /// Stazor専用ロガーのファクトリー
    /// </summary>
    public interface IStazorLoggerFactory
    {
        /// <summary>
        /// Stazor専用ロガーを作成します。
        /// </summary>
        /// <typeparam name="TCategoryName">カテゴリー名</typeparam>
        /// <returns>Stazor専用ロガー</returns>
        IStazorLogger CreateLogger<TCategoryName>();
    }
}