using BookWriting.Domain.Visitors;

namespace BookWriting.Domain.Elements;

public class ParagraphElement : Element
{
    public ParagraphElement(int startIndex, int endIndex) : base(startIndex, endIndex)
    {
    }

    public override bool Equals(object obj)
    {
        return obj is ParagraphElement; // Consider all paragraphs as equal
    }

    public override int GetHashCode()
    {
        // Example implementation
        return 0;  // since all paragraphs are equal
    }

    public override void Accept(IElementVisitor visitor)
    {
        visitor.VisitParagraphElement(this);
    }
}

