using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Stazor.Core
{
    /// <summary>
    /// Helper class for comparing byte arrays.
    /// </summary>
    sealed class ByteArrayComparer : IEqualityComparer<byte[]>
    {
        /// <summary>
        /// Gets a singleton instance of the <see cref="ByteArrayComparer"/>.
        /// </summary>
        public static readonly ByteArrayComparer Default = new();

        /// <inheritdoc/>
        public bool Equals(byte[]? x, byte[]? y) => x.AsSpan().SequenceEqual(y);

        /// <inheritdoc/>
        public int GetHashCode([DisallowNull] byte[] obj)
            => StructuralComparisons.StructuralEqualityComparer.GetHashCode(obj);
    }
}