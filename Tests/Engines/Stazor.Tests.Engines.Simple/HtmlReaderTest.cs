using System;
using System.Text;
using FluentAssertions;
using Stazor.Engines.Simple;
using Xunit;

namespace Stazor.Tests.Engines.Simple
{
    public sealed class HtmlReaderTest
    {
        public static TheoryData<string, MemberSerializer<Block[]>> TestData => new()
        {
            {
                "",
                CreateMembers(new Block(BlockType.None, ""))
            },
            {
                "{", new(new[] { new Block(BlockType.Html, "{") })
            },
            {
                "a", new(new[] { new Block(BlockType.Html, "a") })
            },
            {
                "ab", CreateMembers(new Block(BlockType.Html, "ab"))
            },
            {
                "abc", CreateMembers(new Block(BlockType.Html, "abc"))
            },
            {
                "{ A }", CreateMembers(new Block(BlockType.Html, "{ A }"))
            },
            {
                "z{{A}}z",
                CreateMembers<Block>(new(BlockType.Html, "z"), new(BlockType.Object, "A"), new(BlockType.Html, "z"))
            },
            {
                "z{{ A }}z",
                CreateMembers<Block>(new(BlockType.Html, "z"), new(BlockType.Object, "A"), new(BlockType.Html, "z"))
            },
            {
                "z{{",
                CreateMembers(new Block(BlockType.Html, "z{{"))
            },
            {
                "{{",
                CreateMembers(new Block(BlockType.Html, "{{"))
            },
            {
                "{{ ",
                CreateMembers(new Block(BlockType.Html, "{{ "))
            },
            {
                "{{ A",
                CreateMembers(new Block(BlockType.Html, "{{ A"))
            },
            {
                "}}",
                CreateMembers(new Block(BlockType.Html, "}}"))
            },
            {
                "{{{A}}",
                CreateMembers<Block>(new(BlockType.Html, "{"), new(BlockType.Object, "A"))
            },
            {
                "{{{ A}}",
                CreateMembers<Block>(new(BlockType.Html, "{"), new(BlockType.Object, "A"))
            },
            {
                "{{A}}}",
                CreateMembers<Block>(new(BlockType.Object, "A"), new(BlockType.Html, "}"))
            },
            {
                "{{A }}}",
                CreateMembers<Block>(new(BlockType.Object, "A"), new(BlockType.Html, "}"))
            }
        };

        public sealed class Block
        {
            public object Type { get; set; }

            public string Value { get; set; }

            public Block(object type, string value)
                => (Type, Value) = (type, value);
        }

        [Theory]
        //[InlineData("", "")]
        //[InlineData("{", "{")]
        //[InlineData("a", "a")]
        //[InlineData("ab", "ab")]
        //[InlineData("abc", "abc")]
        //[InlineData("{ A }", "{ A }")]
        //[InlineData("z{{A}}z", "z", "A", "z")]
        //[InlineData("z{{ A }}z", "z", "A", "z")]
        //[InlineData("z{{", "z{{")]
        //[InlineData("{{", "{{")]
        //[InlineData("{{ ", "{{ ")]
        //[InlineData("{{ A", "{{ A")]
        //[InlineData("}}", "}}")]
        //[InlineData("{{{A}}", "{", "A")]
        //[InlineData("{{{ A}}", "{", "A")]
        //[InlineData("{{A}}}", "A", "}")]
        //[InlineData("{{A }}}", "A", "}")]
        [MemberData(nameof(TestData))]
        public void ReadTest(string html, MemberSerializer<Block[]> expectedItems)
        {
            var utf8Html = GetBytes(html);
            var reader = new HtmlReader(utf8Html);
            var count = 0;
            Range range;

            while (reader.Read(out range) is var type && type != BlockType.None)
            {
                var actual = utf8Html[range];
                actual.Should().Equal(GetBytes(expectedItems.Object[count++].Value));
            }

            utf8Html[range].Should().BeEmpty();

            if (!string.IsNullOrEmpty(html))
            {
                count.Should().Be(expectedItems.Object.Length);
            }
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("{", "{")]
        [InlineData("a", "a")]
        [InlineData("ab", "ab")]
        [InlineData("abc", "abc")]
        [InlineData("{ A }", "{ A }")]
        [InlineData("z{{ A }}z", "z")]
        [InlineData("z{{", "z")]
        [InlineData("{{", "")]
        public void ReadHtmlTest(string html, string expected)
        {
            var utf8Html = GetBytes(html);
            var reader = new HtmlReader(utf8Html);
            reader.ReadHtml(out var range);

            var actual = utf8Html[range];
            actual.Should().Equal(GetBytes(expected));
        }

        [Theory]
        [InlineData("", "", false)]
        [InlineData("{", "", false)]
        [InlineData("a", "", false)]
        [InlineData("ab", "", false)]
        [InlineData("abc", "", false)]
        [InlineData("{{", "", false)]
        [InlineData("{{ ", "", false)]
        [InlineData("{{ A", "", false)]
        [InlineData("{A}", "", false)]
        [InlineData("{ A }", "", false)]
        [InlineData("z{{A}}z", "", false)]
        [InlineData("{{{A}}", "", false)]
        [InlineData("{{{ A}}", "", false)]
        [InlineData("{{A}}", "A", true)]
        [InlineData("{{AB}}", "AB", true)]
        [InlineData("{{ A }}", "A", true)]
        [InlineData("{{   A   }}", "A", true)]
        [InlineData("{{ ABC }}", "ABC", true)]
        [InlineData("{{ A B }}", "A B", true)]
        [InlineData("{{A}}}", "A", true)]
        [InlineData("{{A }}}", "A", true)]
        public void TryReadObjectTest(string html, string expected, bool expectedStatus)
        {
            var utf8Html = GetBytes(html);
            var reader = new HtmlReader(utf8Html);
            var status = reader.TryReadObject(out var range);

            var actual = utf8Html[range];
            status.Should().Be(expectedStatus);
            actual.Should().Equal(GetBytes(expected));
        }

        static MemberSerializer<T[]> CreateMembers<T>(params T[] values) => new(values);

        static byte[] GetBytes(string value) => Encoding.UTF8.GetBytes(value);
    }
}
