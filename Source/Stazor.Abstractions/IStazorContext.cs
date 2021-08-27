using SimpleTextTemplate.Abstractions;
using Utf8Utility;

namespace Stazor;

/// <summary>
/// コンテキスト
/// </summary>
public interface IStazorContext : IContext
{
    /// <summary>
    /// 要素を追加します。
    /// </summary>
    /// <param name="key">キー</param>
    /// <param name="value">値</param>
    void Add(Utf8String key, Utf8String value);
}