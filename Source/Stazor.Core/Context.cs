﻿using Utf8Utility;

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
        => new StazorContext(new Utf8ArrayDictionary<Utf8Array>());
}
