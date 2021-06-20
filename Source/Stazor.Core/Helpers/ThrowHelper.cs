using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Stazor.Core.Helpers
{
    static class ThrowHelper
    {
        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowArgumentNullOrWhitespaceException(string paramName)
            => throw new ArgumentException($"The argument '{paramName}' cannot be null, empty or contain only whitespace.", paramName);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowDirectoryNotFoundException()
            => throw new DirectoryNotFoundException();

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowFileNotFoundException(string fileName)
            => throw new FileNotFoundException("File does not exist", fileName);

        /// <summary>
        /// 新しい<see cref="ArgumentNullException"/>例外をスローします。
        /// </summary>
        /// <param name="paramName">引数名</param>
        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowArgumentNullException(string paramName)
            => throw new ArgumentNullException(paramName);
    }
}