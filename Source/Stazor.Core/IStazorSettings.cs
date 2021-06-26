using Utf8Utility;

namespace Stazor.Core
{
    /// <summary>
    /// Stazorで使用するコンテキストキー
    /// </summary>
    public interface IStazorKey
    {
        /// <summary>
        /// コンテキストのキー
        /// </summary>
        Utf8String Key { get; init; }
    }
}