using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Stazor.Plugins.IO.Helpers;

/// <summary>
/// 例外をスローするためのヘルパークラスです。
/// </summary>
static class ThrowHelper
{
    /// <summary>
    /// YAMLの解析に失敗に関する例外をスローします。
    /// </summary>
    /// <param name="position">エラーが発生した位置</param>
    /// <exception cref="YamlParserException">常にこの例外をスローします。</exception>
    [DebuggerHidden]
    [DoesNotReturn]
    public static void ThrowYamlParserException(int position)
        => throw new YamlParserException(position);

    /// <summary>
    /// 対象のファイルが存在しない場合、新しい例外をスローします。
    /// </summary>
    /// <param name="filePath">ファイルパス</param>
    /// <param name="paramName">パラメーター名</param>
    /// <exception cref="ArgumentNullException">ファイルがnullの場合にこの例外をスローします。</exception>
    /// <exception cref="FileNotFoundException">ファイルが存在しない場合にこの例外をスローします。</exception>
    [DebuggerHidden]
    public static void ThrowFileNotFoundExceptionIfFileNotFound(string filePath, [CallerArgumentExpression("filePath")] string? paramName = null)
    {
        ArgumentNullException.ThrowIfNull(filePath, paramName);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Could not find file '{filePath}'.", filePath);
        }
    }
}