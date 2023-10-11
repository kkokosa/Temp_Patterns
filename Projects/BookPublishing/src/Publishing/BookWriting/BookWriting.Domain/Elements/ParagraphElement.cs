using BookWriting.Domain.Visitors;

namespace BookWriting.Domain.Elements;

public class ParagraphElement : Element
{
    private readonly string _text;
    public string Text => _text;
    public ParagraphElement(int startIndex, int endIndex, string text) : base(startIndex, endIndex)
    {
        _text = text;
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

