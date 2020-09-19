using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Stazor.Core
{
    sealed class ByteArrayComparer : IEqualityComparer<byte[]>
    {
        public static readonly ByteArrayComparer Default = new();

        public bool Equals(byte[]? x, byte[]? y) => x.AsSpan().SequenceEqual(y);

        // TODO: パフォーマンス
        public int GetHashCode([DisallowNull] byte[] obj)
            => StructuralComparisons.StructuralEqualityComparer.GetHashCode(obj);
    }
}