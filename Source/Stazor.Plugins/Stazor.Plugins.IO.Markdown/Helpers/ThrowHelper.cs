using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Stazor.Plugins.IO.Helpers
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
        /// 新しい<see cref="DirectoryNotFoundException"/>例外をスローします。
        /// </summary>
        /// <exception cref="DirectoryNotFoundException">常にこの例外をスローします。</exception>
        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowDirectoryNotFoundException()
           => throw new DirectoryNotFoundException();

        /// <summary>
        /// 新しい<see cref="FileNotFoundException"/>例外をスローします。
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <exception cref="FileNotFoundException">常にこの例外をスローします。</exception>
        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowFileNotFoundException(string fileName)
            => throw new FileNotFoundException("File does not exist", fileName);
    }
}