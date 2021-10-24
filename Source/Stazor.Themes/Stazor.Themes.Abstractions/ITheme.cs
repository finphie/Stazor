namespace Stazor.Themes;

/// <summary>
/// テーマには、エンジンやパイプライン、HTML/CSSファイルなどのコンテンツが含まれます。
/// </summary>
public interface ITheme
{
    /// <summary>
    /// テーマの処理を実行します。
    /// </summary>
    /// <returns>テーマの処理を表すタスク</returns>
    ValueTask ExecuteAsync();
}
