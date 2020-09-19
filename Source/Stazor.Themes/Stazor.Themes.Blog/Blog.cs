using System;
using System.Buffers;
using System.IO;
using System.Reflection;
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
        public IEngine Engine => SimpleEngine.Default;

        public Pipeline Pipeline { get; } = new();

        static readonly string TemplatePath =
            Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location)!.FullName, "Layouts/Page.html");

        public Blog(string path)
        {
            Pipeline.Add(new ReadFiles(path, "*.md", TemplatePath));
            Pipeline.Add(new Markdown(ReadFiles.Key));
          
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