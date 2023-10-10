using BookWriting.Domain.Elements;
using BookWriting.Domain.Structure;

namespace BookWriting.Domain.Tests
{
    public class ParserTests
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

            var expectedDocument = new CompositeBuilder("Book")
                .AddParagraph()
                .AddFigure("images/figure1.png", "FigureElement Description")
                .StartChapter("Chapter 1")
                    .AddParagraph()
                .EndChapter()
                .Build();

            Assert.Equal(expectedDocument, document);
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

            var expectedDocument = new CompositeBuilder("Book")
                .AddParagraph()
                .StartChapter("Chapter 1")
                    .AddParagraph()
                    .StartChapter("Chapter 1.1")
                        .AddParagraph()
                    .EndChapter()
                .EndChapter()
                .StartChapter("Chapter 2")
                    .AddParagraph()
                .Build();

            Assert.Equal(expectedDocument, document);
        }
    }

    public class CompositeBuilder
    {
        private readonly Stack<ChapterElement> _chapterStack = new Stack<ChapterElement>();

        public CompositeBuilder(string title)
        {
            var root = new ChapterElement(0, 0, title);  // Create a root chapter.
            _chapterStack.Push(root);
        }

        public CompositeBuilder StartChapter(string title)
        {
            var newChapter = new ChapterElement(0, 0, title);
            _chapterStack.Peek().Add(newChapter);
            _chapterStack.Push(newChapter);  // Push the newly added chapter onto the stack.
            return this;
        }

        public CompositeBuilder EndChapter()
        {
            if (_chapterStack.Count > 1)  // Check to avoid popping the root chapter.
            {
                _chapterStack.Pop();
            }
            return this;
        }

        public CompositeBuilder AddParagraph()
        {
            var paragraph = new ParagraphElement(0, 0);
            _chapterStack.Peek().Add(paragraph);
            return this;
        }

        public CompositeBuilder AddFigure(string path, string caption)
        {
            var figure = new FigureElement(0, 0, path, caption);
            _chapterStack.Peek().Add(figure);
            return this;
        }

        public ChapterElement Build()
        {
            return _chapterStack.Last();  // Return the root chapter.
        }
    }


}