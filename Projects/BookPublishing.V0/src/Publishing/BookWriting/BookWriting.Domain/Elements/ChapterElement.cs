using BookWriting.Domain.Visitors;
using Visitor.SourceGenerators;

namespace BookWriting.Domain.Elements
{
    //[VisitableBy<IElementVisitor>]
    public partial class ChapterElement : Element
    {
        private readonly string _title;
        private readonly Option<string> _subheading;
        private readonly List<Element> _children = new List<Element>();

        public string Title => _title;
        public IReadOnlyList<Element> Components => _children;

        public ChapterElement(int startIndex,
            int endIndex,
            string title)
            : base(startIndex, endIndex)
        {
            _title = title;
            _subheading = Option<string>.None;
        }


        public ChapterElement(int startIndex,
            int endIndex,
            string title,
            Option<string> subheading,
            IEnumerable<Element> children)
            : base(startIndex, endIndex)
        {
            _title = title;
            _subheading = subheading;
            _children.AddRange(children);
        }

        public void Add(Element chapter)
        {
            _children.Add(chapter);
        }

        public override bool Equals(object obj)
        {
            if (obj is ChapterElement otherChapter)
            {
                if (Title != otherChapter.Title || Components.Count != otherChapter.Components.Count)
                    return false;

                for (int i = 0; i < Components.Count; i++)
                {
                    if (!Components[i].Equals(otherChapter.Components[i]))
                        return false;
                }

                return true;
            }

            return false;
        }

        public override int GetHashCode() 
            => Title.GetHashCode() ^ Components.Count;

        public override void Accept(IElementVisitor visitor) 
            => visitor.VisitChapterElement(this);
    }
}