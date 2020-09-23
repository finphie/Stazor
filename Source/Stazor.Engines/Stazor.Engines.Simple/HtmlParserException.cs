using System;

namespace Stazor.Engines.Simple
{
    /// <summary>
    /// Represents error that occurred during HTML parsing.
    /// </summary>
    public sealed class HtmlParserException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlParserException"/> class.
        /// </summary>
        public HtmlParserException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlParserException"/> class.
        /// </summary>
        /// <param name="error">The message that describes the error.</param>
        /// <param name="position">The byte position.</param>
        public HtmlParserException(ParserError error, int position)
            : base($"Error: {error} at position: {position}")
        {
            Error = error;
            Position = position;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlParserException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public HtmlParserException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlParserException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public HtmlParserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Represents the type of error.
        /// </summary>
        public enum ParserError
        {
            /// <summary>
            /// Invalid object format.
            /// </summary>
            InvalidObjectFormat,

            /// <summary>
            /// Expected '{{'.
            /// </summary>
            ExpectedStartObject,

            /// <summary>
            /// Expected '}}'.
            /// </summary>
            ExpectedEndObject
        }

        /// <summary>
        /// Gets the parser error.
        /// </summary>
        /// <value>
        /// The parser error.
        /// </value>
        public ParserError Error { get; }

        /// <summary>
        /// Gets the position of an HTML text.
        /// </summary>
        /// <value>
        /// The byte position.
        /// </value>
        public int Position { get; }
    }
}