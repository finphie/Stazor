using Utf8Utility;

namespace Stazor.Core
{
    /// <summary>
    /// コンテキスト
    /// </summary>
    public interface IContext : SimpleTextTemplate.Abstractions.IContext
    {
        /// <summary>
        /// 要素を追加します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="value">値</param>
        void Add(Utf8String key, Utf8String value);
    }
}