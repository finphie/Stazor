using System.Buffers;
using SimpleTextTemplate;

namespace Stazor.Engines;

/// <summary>
/// シンプルなテンプレートエンジン
/// </summary>
public sealed class SimpleTextTemplateEngine : IEngine
{
    readonly Dictionary<string, Template> _table = new();

    /// <inheritdoc/>
    public string Name => nameof(SimpleTextTemplateEngine);

    /// <inheritdoc/>
    public string Description => "SimpleTextTemplateライブラリを利用したエンジンです。";

    /// <inheritdoc/>
    public ValueTask ExecuteAsync(IBufferWriter<byte> bufferWriter!!, IStazorDocument document!!)
    {
        var path = document.TemplatePath;

        if (_table.TryGetValue(path, out var value))
        {
            value.Render(bufferWriter, document.Context);
            return ValueTask.CompletedTask;
        }

        var file = File.ReadAllBytes(document.TemplatePath);
        var template = Template.Parse(file);
        _table.Add(path, template);

        template.Render(bufferWriter, document.Context);

        return ValueTask.CompletedTask;
    }
}
