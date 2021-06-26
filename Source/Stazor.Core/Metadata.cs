namespace Stazor.Core
{
    /// <summary>
    /// メタデータ作成クラスです。
    /// </summary>
    public static class Metadata
    {
        /// <summary>
        /// メタデータを作成します。
        /// </summary>
        /// <returns>メタデータ</returns>
        public static IStazorMetadata Create()
            => new StazorMetadata();
    }
}