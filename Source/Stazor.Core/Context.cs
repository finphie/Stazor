using Utf8Utility;

namespace Stazor.Core;

/// <summary>
/// コンテキスト作成クラスです。
/// </summary>
public static class Context
{
    /// <summary>
    /// コンテキストを作成します。
    /// </summary>
    /// <returns>コンテキスト</returns>
    public static IStazorContext Create()
        => new StazorContext(new Utf8StringDictionary<Utf8String>());

    /// <summary>
    /// コンテキストを作成します。
    /// </summary>
    /// <param name="symbols">識別子とUTF-8文字列のペアリスト</param>
    /// <returns>コンテキスト</returns>
    public static IStazorContext Create(IUtf8StringDictionary<Utf8String> symbols)
        => new StazorContext(symbols);
}