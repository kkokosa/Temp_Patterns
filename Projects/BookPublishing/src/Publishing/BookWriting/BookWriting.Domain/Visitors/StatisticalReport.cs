using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWriting.Domain.Elements;

namespace BookWriting.Domain.Visitors
{
    public class StatisticalReport : IElementVisitor
    {
        private int _figuresCount = 0;
        private int _wordCount = 0;
        private int _charactersCount = 0;

        public void VisitChapterElement(ChapterElement chapter)
        {
            foreach (var chapterComponent in chapter.Components)
            {
                chapterComponent.Accept(this);
            }
        }

        public void VisitFigureElement(FigureElement figure)
        {
            _figuresCount++;
        }

        public void VisitParagraphElement(ParagraphElement paragraph)
        {
            var text = paragraph.Text;
            var words = text.Split(' ').Length;
            var characters = text.Count(x => char.IsLetterOrDigit(x));
            _wordCount += words;
            _charactersCount += characters;
        }
    }
}
