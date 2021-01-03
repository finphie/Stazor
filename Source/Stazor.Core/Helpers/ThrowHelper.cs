using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Stazor.Core.Helpers
{
    public static class ThrowHelper
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
    }
}