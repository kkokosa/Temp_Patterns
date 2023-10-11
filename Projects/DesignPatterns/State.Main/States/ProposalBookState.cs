using System;

namespace State.Main.States
{
    internal class ProposalBookState : Book.BookStateBase
    {
        public ProposalBookState(Book book) : base(book)
        {
        }

        public override void AddChapter(int index, string title)
        {
            chapters.Add(index, new Chapter(title));
        }

        public override void ChangeMetadata(BookMetadata metadata)
        {
            base.metadata = metadata;
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