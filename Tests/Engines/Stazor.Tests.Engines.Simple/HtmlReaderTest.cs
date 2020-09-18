using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FluentAssertions;
using Stazor.Engines.Simple;
using Xunit;
using static Stazor.Engines.Simple.HtmlParserException;

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
                "{",
                CreateMembers(new Block(BlockType.Html, "{"))
            },
            {
                "a",
                CreateMembers(new Block(BlockType.Html, "a"))
            },
            {
                "ab",
                CreateMembers(new Block(BlockType.Html, "ab"))
            },
            {
                "abc",
                CreateMembers(new Block(BlockType.Html, "abc"))
            },
            {
                "{ A }",
                CreateMembers(new Block(BlockType.Html, "{ A }"))
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

        [Theory]
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
        [InlineData("{", "{")]
        [InlineData("a", "a")]
        [InlineData("ab", "ab")]
        [InlineData("abc", "abc")]
        [InlineData("{ A }", "{ A }")]
        [InlineData("z{{ A }}z", "z")]
        [InlineData("z{{", "z")]
        public void TryReadHtmlTest(string html, string expected)
        {
            var utf8Html = GetBytes(html);
            var reader = new HtmlReader(utf8Html);
            var success = reader.TryReadHtml(out var range);

            var actual = utf8Html[range];
            success.Should().BeTrue();
            actual.Should().Equal(GetBytes(expected));
        }

        [Theory]
        [InlineData("")]
        [InlineData("{{")]
        public void TryReadHtmlTest_Error(string html)
        {
            var utf8Html = GetBytes(html);
            var reader = new HtmlReader(utf8Html);
            var success = reader.TryReadHtml(out var range);

            var actual = utf8Html[range];
            success.Should().BeFalse();
            actual.Should().BeEmpty();
        }

        [Theory]
        [InlineData("{{A}}", "A")]
        [InlineData("{{AB}}", "AB")]
        [InlineData("{{ A }}", "A")]
        [InlineData("{{   A   }}", "A")]
        [InlineData("{{ ABC }}", "ABC")]
        [InlineData("{{ A B }}", "A B")]
        public void ReadObjectTest(string html, string expected)
        {
            var utf8Html = GetBytes(html);
            var reader = new HtmlReader(utf8Html);
            reader.ReadObject(out var range);

            var actual = utf8Html[range];
            actual.Should().Equal(GetBytes(expected));       
        }

        [Theory]
        [InlineData("", ParserError.ExpectedStartObject, 0)]
        [InlineData("{", ParserError.ExpectedStartObject, 0)]
        [InlineData("a", ParserError.ExpectedStartObject, 0)]
        [InlineData("ab", ParserError.ExpectedStartObject, 0)]
        [InlineData("abc", ParserError.ExpectedStartObject, 0)]
        [InlineData("z{{A}}z", ParserError.ExpectedStartObject, 0)]
        [InlineData("{A}", ParserError.ExpectedStartObject, 1)]
        [InlineData("{ A }", ParserError.ExpectedStartObject, 1)]
        [InlineData("{{{", ParserError.ExpectedStartObject, 2)]
        [InlineData("{{{A}}", ParserError.ExpectedStartObject, 2)]
        [InlineData("{{{ A}}", ParserError.ExpectedStartObject, 2)]
        [InlineData("{{", ParserError.ExpectedEndObject, 1)]
        [InlineData("{{ ", ParserError.ExpectedEndObject, 2)]
        [InlineData("{{ A", ParserError.ExpectedEndObject, 3)]
        [InlineData("{{A}}}", ParserError.ExpectedEndObject, 5)]
        [InlineData("{{A }}}", ParserError.ExpectedEndObject, 6)]
        public void ReadObjectTest_Error(string html, ParserError error, int position)
        {
            FluentActions.Invoking(Execute).Should().Throw<HtmlParserException>()
                .Where(x => x.Error == error && x.Position == position);

            Range Execute()
            {
                var utf8Html = GetBytes(html);
                var reader = new HtmlReader(utf8Html);
                reader.ReadObject(out var range);

                return range;
            }
        }

        static MemberSerializer<T[]> CreateMembers<T>(params T[] values) => new(values);

        static byte[] GetBytes(string value) => Encoding.UTF8.GetBytes(value);

        [SuppressMessage("Style", "IDE1006:命名スタイル", Justification = "record parameters")]
        public record Block(object Type, string Value);
    }
}