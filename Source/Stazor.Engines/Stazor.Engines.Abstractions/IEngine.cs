using System.Buffers;

namespace Stazor.Engines;

/// <summary>
/// テンプレートエンジンを表します。
/// </summary>
public interface IEngine
{
    /// <summary>
    /// エンジン名を取得します。
    /// </summary>
    /// <value>
    /// エンジン名
    /// </value>
    string Name { get; }

    /// <summary>
    /// エンジンの説明を取得します。
    /// </summary>
    /// <value>
    /// エンジンの説明
    /// </value>
    string Description { get; }

    /// <summary>
    /// エンジンの処理を実行します。
    /// </summary>
    /// <param name="bufferWriter">ターゲットのwriter</param>
    /// <param name="document">ドキュメント</param>
    /// <returns>エンジンの処理を表すタスク</returns>
    ValueTask ExecuteAsync(IBufferWriter<byte> bufferWriter, IStazorDocument document);
}