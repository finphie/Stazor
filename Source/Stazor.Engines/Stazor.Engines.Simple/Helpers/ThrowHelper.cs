using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static Stazor.Engines.Simple.HtmlParserException;

namespace Stazor.Engines.Simple.Helpers
{
    static class ThrowHelper
    {
        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowHtmlParserException(ParserError error, int position)
            => throw new HtmlParserException(error, position);
    }
}