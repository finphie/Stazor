using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Stazor.Plugins.IO;

/// <summary>
/// SortedSet resolver.
/// </summary>
sealed class SortedSetResolver : INodeTypeResolver
{
    /// <summary>
    /// Gets a singleton instance of the <see cref="SortedSetResolver"/>.
    /// </summary>
    public static readonly SortedSetResolver Default = new();

    /// <inheritdoc/>
    public bool Resolve(NodeEvent? nodeEvent, ref Type currentType)
    {
        if (currentType != typeof(IReadOnlySet<string>))
        {
            return false;
        }

        currentType = typeof(SortedSet<string>);
        return true;
    }
}