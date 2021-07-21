namespace Stazor.Themes
{
    /// <summary>
    /// <see cref="SimpleBlog"/>の設定クラスです。
    /// </summary>
    public sealed record SimpleBlogSettings : ThemeBaseSettings, ISettingsKey
    {
        /// <summary>
        /// キー
        /// </summary>
        public static string Key => nameof(SimpleBlog);
    }
}