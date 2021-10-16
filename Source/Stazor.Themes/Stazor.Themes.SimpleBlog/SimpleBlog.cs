using System.Buffers;
using System.Text;
using Stazor.Engines;

namespace Stazor.Themes;

/// <summary>
/// シンプルブログ
/// </summary>
public sealed class SimpleBlog : ITheme
{
    readonly IEngine _engine;
    readonly IPipeline _pipeline;
    readonly SimpleBlogSettings _settings;

    /// <summary>
    /// <see cref="SimpleBlog"/>クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="engine">エンジン</param>
    /// <param name="pipeline">パイプライン</param>
    /// <param name="settings">設定</param>
    public SimpleBlog(IEngine engine, IPipeline pipeline, SimpleBlogSettings settings)
    {
        _engine = engine ?? throw new ArgumentNullException(nameof(engine));
        _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    /// <inheritdoc/>
    public async ValueTask ExecuteAsync()
    {
        var filePaths = Directory.GetFiles(_settings.ContentPath, "*.md");
        var buffer = new ArrayBufferWriter<byte>();
        var documents = _pipeline.Execute(filePaths);

        int i;
        for (i = 0; i < documents.Length; i++)
        {
            await _engine.ExecuteAsync(buffer, documents[i]).ConfigureAwait(false);

            Console.Write(Encoding.UTF8.GetString(buffer.WrittenSpan).Length + ",");

            // using var fs = new FileStream($"g/{i}.html", FileMode.Create, FileAccess.Write, FileShare.Read);
            // fs.Write(buffer.WrittenSpan);
            buffer.Clear();
        }

        Console.WriteLine();
        Console.WriteLine(i);
    }
}