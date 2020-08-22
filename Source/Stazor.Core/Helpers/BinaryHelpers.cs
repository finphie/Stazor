using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Stazor.Core.Helpers
{
    static class BinaryHelpers
    {
        public static void Copy(Span<byte> destination, in ReadOnlySpan<byte> source)
        {
            ref var destinationStart = ref MemoryMarshal.GetReference(destination);
            ref var sourceStart = ref MemoryMarshal.GetReference(source);

            Unsafe.CopyBlockUnaligned(ref destinationStart, ref sourceStart, (uint)source.Length);
        }
    }
}