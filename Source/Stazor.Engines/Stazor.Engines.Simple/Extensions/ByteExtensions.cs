using System.Runtime.CompilerServices;

namespace Stazor.Engines.Simple.Extensions
{
    /// <summary>
    /// Helper class for handling <see cref="byte"/>.
    /// </summary>
    static class ByteExtensions
    {
        /// <summary>
        /// Determines whether a byte is whitespace.
        /// </summary>
        /// <param name="value">The UTF-8 encoded byte.</param>
        /// <returns>
        /// <see langword="true"/> if a byte is whitespace.
        /// Otherwise, returns <see langword="false"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsWhiteSpace(this byte value) => value == (byte)' ';
    }
}