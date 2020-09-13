using System;
using System.Buffers;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Stazor.Core;
using Stazor.Engine;
using Stazor.Engines.Simple;
using Stazor.Plugins.IO;
using Stazor.Plugins.Metadata;
using Stazor.Plugins.Renderer;

namespace Stazor.Themes
{
    public sealed class Blog : ITheme
    {
        public IEngine Engine { get; }

        public Pipeline Pipeline { get; } = new();

        public Blog(string path)
        {
            Engine = new SimpleEngine(@"Stazor.Themes.Blog");

            Pipeline.Add(new ReadFiles(path, "*.md", "Page.html"));
            Pipeline.Add(new Markdown(nameof(ReadFiles)));
          
            //Pipeline.Add(Viewport.Default);
            //Pipeline.Add(new StyleSheet("style.css"));
            //Pipeline.Add(new Favicon("/favicon.svg"));
            //Pipeline.Add(new Breadcrumb());
        }

        public async ValueTask ExecuteAsync()
        {
            var buffer = new ArrayBufferWriter<byte>();
            var count = 0;

            await foreach (var document in Pipeline.ExecuteAsync().ConfigureAwait(false))
            {
                await Engine.ExecuteAsync(buffer, document).ConfigureAwait(false);

                Console.WriteLine(Encoding.UTF8.GetString(buffer.WrittenSpan));

                //using var fs = new FileStream($"{count++}.html", FileMode.Create, FileAccess.Write, FileShare.Read);
                //fs.Write(buffer.WrittenSpan);

                buffer.Clear();
            }
        }
    }
}