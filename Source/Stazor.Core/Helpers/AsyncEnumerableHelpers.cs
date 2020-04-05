using System.Collections.Generic;

namespace Stazor.Core.Helpers
{
    static class AsyncEnumerableHelpers
    {
#pragma warning disable CS1998 // 非同期メソッドは、'await' 演算子がないため、同期的に実行されます
        public static async IAsyncEnumerable<T> Empty<T>()
#pragma warning restore CS1998 // 非同期メソッドは、'await' 演算子がないため、同期的に実行されます
        {
            yield break;
        }
    }
}