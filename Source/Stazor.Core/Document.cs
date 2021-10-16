using Stazor.Core.Helpers;

namespace Stazor.Core;

/// <summary>
/// <see cref="IStazorDocument"/>の新しいインスタンスを作成します。
/// </summary>
public static class Document
{
    /// <summary>
    /// <see cref="StazorDocument"/>クラスの新しいインスタンスを作成します。
    /// </summary>
    /// <param name="templatePath">テンプレートディレクトリのパス</param>
    /// <param name="metadata">メタデータ</param>
    /// <returns>ドキュメントインスタンス</returns>
    public static IStazorDocument Create(string templatePath, IStazorMetadata metadata)
    {
        ThrowHelper.ThrowDirectoryNotFoundExceptionIfDirectoryNotFound(templatePath);
        ArgumentNullException.ThrowIfNull(metadata);

        return new StazorDocument(templatePath, Context.Create(), metadata);
    }

    /// <summary>
    /// <see cref="StazorDocument"/>クラスの配列を作成します。
    /// </summary>
    /// <param name="length">長さ</param>
    /// <returns>ドキュメントのリスト</returns>
    public static IStazorDocument[] CreateArray(int length)
    {
        ThrowHelper.ThrowArgumentOutOfRangeExceptionIfNegativeNumber(length);
        return new StazorDocument[length];
    }
}