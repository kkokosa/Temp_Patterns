using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWriting.Domain.Elements;

namespace BookWriting.Domain.Visitors
{
    public class TocGenerator : IElementVisitor
    {
        private readonly StringBuilder _toc = new StringBuilder();
        
        private int _identationLevel = 0;

        public void VisitChapterElement(ChapterElement chapter)
        {
            var identation = new string(' ', _identationLevel * 3);
            _toc.AppendLine($"{identation}{chapter.Title}");
            foreach (var chapterComponent in chapter.Components)
            {
                _identationLevel++;
                chapterComponent.Accept(this);
                _identationLevel--;
            }
        }

        public void VisitFigureElement(FigureElement figure)
        {
            // Do not considered in TOC
        }

        public void VisitParagraphElement(ParagraphElement paragraph)
        {
            // Do not considered in TOC
        }

        public string GetResult() => _toc.ToString();
    }
}
