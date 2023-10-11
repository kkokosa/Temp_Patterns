using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWriting.Domain.Elements;

namespace BookWriting.Domain.Visitors
{
    public interface IElementVisitor
    {
        void VisitChapterElement(ChapterElement chapter);
        void VisitFigureElement(FigureElement figure);
        void VisitParagraphElement(ParagraphElement paragraph);
    }
}
