using System.Diagnostics.CodeAnalysis;

namespace Stazor.Core;

/// <summary>
/// メタデータ
/// </summary>
public sealed record StazorMetadata : IStazorMetadata
{
    /// <inheritdoc/>
    [AllowNull]
    public string Title { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset PublishedDate { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset ModifiedDate { get; set; }

    /// <inheritdoc/>
    [AllowNull]
    public string Category { get; set; }

    /// <inheritdoc/>
    [AllowNull]
    public IReadOnlySet<string> Tags { get; set; }
}