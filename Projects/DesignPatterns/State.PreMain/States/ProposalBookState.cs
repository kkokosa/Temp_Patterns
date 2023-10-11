using System;

namespace State.PreMain.States
{
    internal class ProposalBookState : BookStateBase
    {
        public ProposalBookState(Book book) : base(book)
        {
        }

        public override void AddChapter(int index, string title)
        {
            book.Chapters.Add(index, new Chapter(title));
        }

        public override void ChangeMetadata(BookMetadata metadata)
        {
            book.Metadata = metadata;
        }

        public override void GenerateIndex()
        {
            throw new InvalidOperationException();
        }

        public override void GenerateTableOfContent()
        {
            throw new InvalidOperationException();
        }

        public override void Print()
        {
            throw new InvalidOperationException();
        }
    }
}