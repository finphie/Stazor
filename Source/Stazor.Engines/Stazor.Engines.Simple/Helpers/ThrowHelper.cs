using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static Stazor.Engines.Simple.HtmlParserException;

namespace Stazor.Engines.Simple.Helpers
{
    /// <summary>
    /// Helper methods for throwing exceptions.
    /// </summary>
    static class ThrowHelper
    {
        /// <summary>
        /// Throws a new <see cref="HtmlParserException"/> exception.
        /// </summary>
        /// <param name="error">The parser error.</param>
        /// <param name="position">The byte position.</param>
        /// <exception cref="HtmlParserException">Always throws this.</exception>
        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowHtmlParserException(ParserError error, int position)
            => throw new HtmlParserException(error, position);
    }
}