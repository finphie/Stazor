using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Stazor.Core.Helpers
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

        /// <summary>
        /// 新しい<see cref="ArgumentException"/>例外をスローします。
        /// </summary>
        /// <param name="paramName">引数名</param>
        /// <exception cref="ArgumentException">常にこの例外をスローします。</exception>
        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowArgumentNullOrWhitespaceException(string paramName)
            => throw new ArgumentException($"The argument '{paramName}' cannot be null, empty or contain only whitespace.", paramName);

        /// <summary>
        /// 新しい<see cref="ArgumentNullException"/>例外をスローします。
        /// </summary>
        /// <typeparam name="T">キーの型</typeparam>
        /// <param name="key">キー名</param>
        /// <exception cref="ArgumentNullException">常にこの例外をスローします。</exception>
        public static void ThrowAddingDuplicateWithKeyArgumentException<T>(T key)
            => throw new ArgumentException($"An item with the same key has already been added. Key: {key}");
    }
}