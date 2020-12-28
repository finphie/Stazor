using System;

namespace Stazor.Core.Helpers
{
    public static class ThrowHelper
    {
        public static ArgumentException CreateArgumentNullOrWhitespaceException(string paramName)
            => CreateArgumentException($"The argument '{paramName}' cannot be null, empty or contain only whitespace.", paramName);

        static ArgumentException CreateArgumentException(string message, string paramName)
            => new(message, paramName);
    }
}