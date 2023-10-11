namespace Composite.Main
{
    public abstract class Part
    {
        public int StartIndex { get; }
        public int EndIndex { get; }

        protected Part(int startIndex, int endIndex)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
    }

    public class Paragraph : Part
    {
        public Paragraph(int startIndex, int endIndex) : base(startIndex, endIndex)
        {
        }
    }

    public class Figure : Part
    {
        private readonly string _source;

        public Figure(string source, int startIndex, int endIndex) : base(startIndex, endIndex)
        {
            _source = source;
        }
    }

    public class Chapter : Part
    {
        private readonly string _title;
        private readonly List<Part> _children = new List<Part>();

        public Chapter(string title, int startIndex, int endIndex) : base(startIndex, endIndex)
        {
            _title = title;
        }

        public void Add(Part component)
        {
            _children.Add(component);
        }
    }
}
