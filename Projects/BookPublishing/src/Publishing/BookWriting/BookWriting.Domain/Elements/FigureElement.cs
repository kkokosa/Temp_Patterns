using BookWriting.Domain.Visitors;

namespace BookWriting.Domain.Elements;

public class FigureElement : Element
{
    private readonly string _source;
    private readonly string _caption;

    public string Source => _source;
    public string Caption => _caption;

    public FigureElement(int startIndex, int endIndex, string source, string caption) : base(startIndex, endIndex)
    {
        _source = source;
        _caption = caption;
    }

    public override bool Equals(object obj)
    {
        if (obj is FigureElement otherFigure)
            return Source == otherFigure.Source && Caption == otherFigure.Caption;
        return false;
    }

    public override int GetHashCode() 
        => HashCode.Combine(Source.GetHashCode(), Caption.GetHashCode());

    public override void Accept(IElementVisitor visitor)
        => visitor.VisitFigureElement(this);
}