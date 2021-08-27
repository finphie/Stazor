namespace Stazor.Logging;

/// <summary>
/// Stazor専用ロガー
/// </summary>
public interface IStazorLogger
{
    /// <summary>
    /// トレースメッセージを出力します。
    /// </summary>
    /// <param name="message">メッセージ</param>
    void Trace(string message);

    /// <summary>
    /// デバッグメッセージを出力します。
    /// </summary>
    /// <param name="message">メッセージ</param>
    void Debug(string message);

    /// <summary>
    /// 情報メッセージを出力します。
    /// </summary>
    /// <param name="message">メッセージ</param>
    void Information(string message);

    /// <summary>
    /// 警告メッセージを出力します。
    /// </summary>
    /// <param name="message">メッセージ</param>
    void Warning(string message);

    /// <summary>
    /// エラーメッセージを出力します。
    /// </summary>
    /// <param name="message">メッセージ</param>
    void Error(string message);

    /// <summary>
    /// 致命的なエラーメッセージを出力します。
    /// </summary>
    /// <param name="message">メッセージ</param>
    void Critical(string message);
}

/// <summary>
/// Stazor専用ロガー
/// </summary>
/// <typeparam name="TCategoryName">カテゴリー名</typeparam>
public interface IStazorLogger<out TCategoryName> : IStazorLogger
{
}