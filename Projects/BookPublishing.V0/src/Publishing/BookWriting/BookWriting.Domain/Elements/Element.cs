using BookWriting.Domain.Visitors;

namespace BookWriting.Domain.Elements;

public abstract class Element
{
    public int StartIndex { get; }
    public int EndIndex { get; }

    protected Element(int startIndex, int endIndex)
    {
        StartIndex = startIndex;
        EndIndex = endIndex;
    }

    public abstract void Accept(IElementVisitor visitor);
}