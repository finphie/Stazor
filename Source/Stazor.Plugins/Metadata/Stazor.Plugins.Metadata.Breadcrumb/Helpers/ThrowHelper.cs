using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Stazor.Plugins.Metadata.Helpers
{
    /// <summary>
    /// 例外をスローするためのヘルパークラスです。
    /// </summary>
    static class ThrowHelper
    {
        /// <summary>
        /// 新しい<see cref="ArgumentNullException"/>例外をスローします。
        /// </summary>
        /// <param name="paramName">引数名</param>
        /// <exception cref="ArgumentNullException">常にこの例外をスローします。</exception>
        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowArgumentNullException(string paramName)
            => throw new ArgumentNullException(paramName);
    }
}