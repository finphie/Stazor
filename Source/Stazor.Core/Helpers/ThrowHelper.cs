using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Stazor.Core.Helpers;

/// <summary>
/// 例外をスローするためのヘルパークラスです。
/// </summary>
static class ThrowHelper
{
    /// <summary>
    /// 新しい<see cref="ArgumentNullException"/>例外をスローします。
    /// </summary>
    /// <typeparam name="T">キーの型</typeparam>
    /// <param name="key">キー名</param>
    /// <exception cref="ArgumentNullException">常にこの例外をスローします。</exception>
    [DebuggerHidden]
    [DoesNotReturn]
    public static void ThrowAddingDuplicateWithKeyArgumentException<T>(T key)
        => throw new ArgumentException($"An item with the same key has already been added. Key: {key}");

    /// <summary>
    /// 新しい<see cref="ArgumentNullException"/>例外をスローします。
    /// </summary>
    /// <param name="paramName">引数名</param>
    /// <exception cref="ArgumentNullException">常にこの例外をスローします。</exception>
    [DebuggerHidden]
    [DoesNotReturn]
    public static void ThrowInvalidDateException(string paramName)
        => throw new ArgumentException($"{paramName}が不正な日時です。");

    /// <summary>
    /// 対象の文字列がnullまたは空白の場合、新しい<see cref="ArgumentException"/>例外をスローします。
    /// </summary>
    /// <param name="argument">対象の文字列</param>
    /// <param name="paramName">引数名</param>
    /// <exception cref="ArgumentException">常にこの例外をスローします。</exception>
    [DebuggerHidden]
    public static void ThrowArgumentNullOrWhitespaceExceptionIfNullOrWhitespace(string argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (string.IsNullOrWhiteSpace(argument))
        {
            throw new ArgumentException($"The argument '{paramName}' cannot be null, empty or contain only whitespace.", paramName);
        }
    }

    /// <summary>
    /// 対象のパスが存在しない場合、新しい例外をスローします。
    /// </summary>
    /// <param name="path">パス</param>
    /// <param name="paramName">パラメーター名</param>
    /// <exception cref="ArgumentNullException">パスがnullの場合にこの例外をスローします。</exception>
    /// <exception cref="DirectoryNotFoundException">パスが存在しない場合にこの例外をスローします。</exception>
    [DebuggerHidden]
    public static void ThrowDirectoryNotFoundExceptionIfDirectoryNotFound(string path, [CallerArgumentExpression("path")] string? paramName = null)
    {
        ArgumentNullException.ThrowIfNull(path, paramName);

        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException($"Could not find a part of the path '{path}'.");
        }
    }
}