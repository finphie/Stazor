using Stazor.Plugins.Metadata.Helpers;
using Utf8Utility;

namespace Stazor.Plugins.Metadata
{
    /// <summary>
    /// <see cref="Breadcrumb"/>プラグインの設定クラスです。
    /// </summary>
    public sealed record BreadcrumbSettings : ISettingsKey
    {
        public static string Key => nameof(Breadcrumb);

        /// <summary>
        /// コンテキストのキーを取得または設定します。
        /// </summary>
        /// <value>
        /// コンテキストのキー
        /// </value>
        public string ContextKey { get; set; }

        public Utf8String JsonLdKey = (Utf8String)"JsonLd";

        /// <summary>
        /// JSON-LDの出力可否を取得または設定します。
        /// </summary>
        /// <value>
        /// JSON-LDの出力可否
        /// </value>
        public bool JsonLd { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Key))
            {
                ThrowHelper.ThrowArgumentNullException(nameof(Key));
            }
        }
    }
}