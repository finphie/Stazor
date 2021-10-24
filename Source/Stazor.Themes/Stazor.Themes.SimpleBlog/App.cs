using ConsoleAppFramework;
using Stazor.Logging;

namespace Stazor.Themes;

/// <summary>
/// コマンド
/// </summary>
public sealed class App : ConsoleAppBase
{
    readonly IStazorLogger _logger;
    readonly ITheme _theme;

    /// <summary>
    /// <see cref="App"/>クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="logger">ロガー</param>
    /// <param name="theme">テーマ</param>
    public App(IStazorLogger<App> logger, ITheme theme)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _theme = theme ?? throw new ArgumentNullException(nameof(theme));
    }

    /// <summary>
    /// ビルドを行います。
    /// </summary>
    /// <returns>ビルドを表すタスク</returns>
    [Command("build")]
    public async ValueTask BuildAsync()
    {
        _logger.Information("Start");

        await _theme.ExecuteAsync().ConfigureAwait(false);

        _logger.Information("End");
    }
}
