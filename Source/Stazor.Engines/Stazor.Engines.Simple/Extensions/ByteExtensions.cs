using System.Runtime.CompilerServices;

namespace Stazor.Engines.Simple.Extensions
{
    static class ByteExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsWhiteSpace(this byte value) => value == (byte)' ';
    }
}