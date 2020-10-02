using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Stazor.Core.Helpers
{
    /// <summary>
    /// Helper class for handling <see cref="IAsyncEnumerable{T}"/>.
    /// </summary>
    static class AsyncEnumerableHelpers
    {
        /// <summary>
        /// Represents the empty <see cref="IAsyncEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type used for the <see cref="IAsyncEnumerable{T}"/> type parameter of the resulting sequence.</typeparam>
        /// <returns>A sequence with no elements.</returns>
#pragma warning disable CS1998 // 非同期メソッドは、'await' 演算子がないため、同期的に実行されます
        [SuppressMessage("Style", "IDE1006:命名スタイル", Justification = "Empty")]
        [SuppressMessage("Style", "VSTHRD200:非同期メソッドに \"Async\" サフィックスを使用する", Justification = "Empty")]
        public static async IAsyncEnumerable<T> Empty<T>()
#pragma warning restore CS1998 // 非同期メソッドは、'await' 演算子がないため、同期的に実行されます
        {
            yield break;
        }
    }
}