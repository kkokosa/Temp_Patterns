using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWriting.Domain.Structure;
using BookWriting.Domain.Visitors;

namespace BookWriting.Domain.Tests
{
    public class TocTests
    {
        [Fact]
        public void ParsingSimpleMarkdown()
        {
            var markdownText = @"# Book

This is an introduction paragraph.

![FigureElement Description](images/figure1.png)

## Chapter 1

This is content inside ChapterElement 1.
";
            var lexer = new Lexer(markdownText);
            var parser = new MarkdownParser(lexer);
            var document = parser.Parse();

            var tocBuilder = new TocGenerator();
            document.Accept(tocBuilder);
            var toc = tocBuilder.GetResult(); 

            var expectedToc =
@"Book
   Chapter 1
";

            Assert.Equal(expectedToc, toc);
        }

        [Fact]
        public void ParsingThreeLevelMarkdown()
        {
            var markdownText = @"# Book

This is an introduction paragraph.

## Chapter 1

This is content inside ChapterElement 1.

### Chapter 1.1

This is content inside ChapterElement 1.1.

## Chapter 2

Almost the end.
";
            var lexer = new Lexer(markdownText);
            var parser = new MarkdownParser(lexer);
            var document = parser.Parse();

            var tocBuilder = new TocGenerator();
            document.Accept(tocBuilder);
            var toc = tocBuilder.GetResult();

            var expectedToc = 
@"Book
   Chapter 1
      Chapter 1.1
   Chapter 2
";
            Assert.Equal(expectedToc, toc);
        }
    }
}
