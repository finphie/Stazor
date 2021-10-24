namespace Stazor.Plugins.IO;

/// <summary>
/// <see cref="YamlFrontMatterReader"/>構造体に関する例外クラスです。
/// </summary>
sealed class YamlParserException : Exception
{
    /// <summary>
    /// <see cref="YamlParserException"/>クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="position">エラーが発生した位置</param>
    public YamlParserException(int position)
        : base($"Error: Invalid data at position: {position}")
        => Position = position;

    /// <summary>
    /// エラーが発生した位置を取得します。
    /// </summary>
    /// <value>
    /// エラーが発生した位置
    /// </value>
    public int Position { get; }
}
