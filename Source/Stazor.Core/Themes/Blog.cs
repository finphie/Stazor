using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Text;
using Stazor.Core.Pipelines;
using Stazor.Core.Plugins.IO;
using Stazor.Core.Plugins.Metadata;
using Stazor.Core.Plugins.Renderer;

namespace Stazor.Core.Themes
{
    public sealed class Blog : ITheme
    {
        static readonly byte[] StartHtml = Encoding.UTF8.GetBytes("<html>");
        static readonly byte[] EndHtml = Encoding.UTF8.GetBytes("</html>");
        static readonly byte[] StartHead = Encoding.UTF8.GetBytes("<head>");
        static readonly byte[] EndHead = Encoding.UTF8.GetBytes("</head>");
        static readonly byte[] StartBody = Encoding.UTF8.GetBytes("<body>");
        static readonly byte[] EndBody = Encoding.UTF8.GetBytes("</body>");
        static readonly byte[] StartMain = Encoding.UTF8.GetBytes("<main>");
        static readonly byte[] EndMain = Encoding.UTF8.GetBytes("</main>");
        static readonly byte[] StartArticle = Encoding.UTF8.GetBytes("<article>");
        static readonly byte[] EndArticle = Encoding.UTF8.GetBytes("</article>");
        static readonly byte[] StartHeader = Encoding.UTF8.GetBytes("<header>");
        static readonly byte[] EndHeader = Encoding.UTF8.GetBytes("</header>");

        public Pipeline Pipeline { get; } = new();

        public Blog(string path)
        {
            Pipeline.Add(new ReadFiles(path, "*.md"));
            Pipeline.Add(Markdown.Default);
          
            Pipeline.Add(Viewport.Default);
            Pipeline.Add(new StyleSheet("style.css"));
            Pipeline.Add(new Favicon("/favicon.svg"));
            Pipeline.Add(new Breadcrumb());
        }

        public async ValueTask ExecuteAsync()
        {
            var buffer = new ArrayBufferWriter<byte>();
            var count = 0;

            await foreach (var document in Pipeline.ExecuteAsync().ConfigureAwait(false))
            {
                buffer.Write(StartHtml);

                buffer.Write(StartHead);
                buffer.Write(document.Content.Head.WrittenSpan);
                buffer.Write(EndHead);

                buffer.Write(StartBody);

                if (document.Content.Body.Header.WrittenCount != 0)
                {
                    buffer.Write(StartHeader);
                    buffer.Write(document.Content.Body.Header.WrittenSpan);
                    buffer.Write(EndHeader);
                }

                buffer.Write(StartMain);

                if (document.Content.Body.Main.Header.WrittenCount != 0)
                {
                    buffer.Write(StartHeader);
                    buffer.Write(document.Content.Body.Main.Header.WrittenSpan);
                    buffer.Write(EndHeader);
                }

                if (document.Content.Body.Main.Article.WrittenCount != 0)
                {
                    buffer.Write(StartArticle);
                    buffer.Write(document.Content.Body.Main.Article.WrittenSpan);
                    buffer.Write(EndArticle);
                }

                buffer.Write(EndMain);
                buffer.Write(EndBody);
                buffer.Write(EndHtml);

                Console.WriteLine(Encoding.UTF8.GetString(buffer.WrittenSpan));

                using var fs = new FileStream($"{count++}.html", FileMode.Create, FileAccess.Write, FileShare.Read);
                fs.Write(buffer.WrittenSpan);

                buffer.Clear();
            }
        }
    }
}