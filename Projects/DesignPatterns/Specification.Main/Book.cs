using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp
{

    public class Book
    {
        readonly int ConstantMarginWords = 2;

        private readonly Specification<Book> eligableForProduction =
            new BookStageSpecification(stage: BookStage.Draft);
        private readonly Specification<Book> eligableForFinal =
            new BookStageSpecification(stage: BookStage.Production);

        private readonly Specification<Book> eligableForMetadataModification =
            new BookStageSpecification(stage: BookStage.Draft);
        private readonly Specification<Book> eligableForPriceCalculation =
            new BookStageSpecification(stage: BookStage.Draft)
            .Or(new BookStageSpecification(stage: BookStage.Production));

        private readonly List<string> chapters;

        public IReadOnlyList<string> Chapters => chapters.AsReadOnly();
        public string TableOfContents { get; private set; }
        public string Author { get; private set; }
        public OptionalDateTime PublicationDate { get; private set; }
        public BookStage Stage { get; private set; }

        public Book()
        {
            this.chapters = new List<string>();
            this.Stage = BookStage.Draft;
            this.PublicationDate = OptionalDateTime.Unspecified;
        }

        public void CalculateTableOfContents()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < Chapters.Count; i++)
            {
                var chapter = Chapters[i];
                var title = ExtractTitleFromChapter(chapter);
                builder.AppendLine($"{i}. {title}");
            }
            TableOfContents = builder.ToString();
        }

        public int CalculateTotalSize(bool excludeTableOfContents)
        {
            int result = Chapters.Select(x => x.Length).Sum() + this.ConstantMarginWords;
            if (!excludeTableOfContents)
            {
                if (string.IsNullOrEmpty(TableOfContents))
                    throw new InvalidBookOperationException("Missing TOC");
                result += TableOfContents.Length;
            }
            return result;
        }

        public void StartProduction()
        {
            if (!eligableForProduction.IsSatisfiedBy(this))
                throw new InvalidBookStageException(expected: BookStage.Draft, actual: Stage);

            Stage = BookStage.Production;
        }

        public void StartFinal(string date)
        {
            if (!eligableForFinal.IsSatisfiedBy(this))
                throw new InvalidBookStageException(expected: BookStage.Production, actual: Stage);

            Stage = BookStage.Final;
            PublicationDate.Value = DateTime.Parse(date);
        }
    
        public void AddChapter(string title, string content)
        {
            if (!eligableForMetadataModification.IsSatisfiedBy(this))
                throw new InvalidBookStageException(expected: BookStage.Draft, actual: Stage);

            this.chapters.Add(string.Format("# {0}\r\n{1}", title, content));
        }

        public void SetAuthor(string author)
        {
            if (!eligableForMetadataModification.IsSatisfiedBy(this))
                throw new InvalidBookStageException(actual: Stage);

            this.Author = author;
        }

        public void Print(BookFormat format)
        {
            switch (format)
            {
                case BookFormat.Text:
                    PrintTextFormat();
                    break;
                case BookFormat.PDF :
                default:
                    throw new NotSupportedException();
            }
        }

        private void PrintTextFormat()
        {
            StringBuilder book = new StringBuilder(CalculateTotalSize(excludeTableOfContents: false));
            book.Append(TableOfContents);
            this.chapters.ForEach(x => book.Append(x));
            File.WriteAllText(@"C:\temp\output.txt", book.ToString());
        }

        private static string ExtractTitleFromChapter(string chapter)
        {
            // Get the title text from the first line starting with #
            var match = Regex.Match(chapter, @"^#\s*(.*?)\s*$", RegexOptions.Multiline);
            if (!match.Success || match.Groups.Count < 2)
                throw new InvalidChapterFormatException("Missing title tag");
            return match.Groups[1].Value;
        }
    }
}
