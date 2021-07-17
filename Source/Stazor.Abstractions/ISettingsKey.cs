namespace Stazor
{
    /// <summary>
    /// 設定クラスのキー
    /// </summary>
    public interface ISettingsKey
    {
        /// <summary>
        /// キーを取得または設定します。
        /// </summary>
        /// <value>
        /// キー
        /// </value>
        static abstract string Key { get; }
    }
}