using System;

namespace Stazor.Core
{
    public interface IDocument
    {
        /*
        string Id { get; }

        DateTimeOffset Date { get; }

        string Category { get; }

        string Title { get; }
        */

        string Content { get; }
    }
}