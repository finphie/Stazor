using System;

namespace Stazor.Engines.Simple
{
    public sealed class HtmlParserException : Exception
    {
        public ParserError Error { get; }

        public int Position { get; }

        /// <summary>
        /// Creates a new exception object.
        /// </summary>
        public HtmlParserException() { }

        /// <summary>
        /// Creates a new exception object.
        /// </summary>
        /// <param name="error"></param>
        /// <param name="position"></param>
        public HtmlParserException(ParserError error, int position)
            : base($"Error: {error} at position: {position}")
        {
            Error = error;
            Position = position;
        }

        public enum ParserError
        {
            InvalidObjectFormat,
            ExpectedStartObject,
            ExpectedEndObject
        }
    }
}