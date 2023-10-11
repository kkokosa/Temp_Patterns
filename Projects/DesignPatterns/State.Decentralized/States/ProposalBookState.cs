using System;

namespace State.Decentralized.States
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

        public override void MoveToState(State state)
        {
            if (state != State.Draft)
                throw new InvalidOperationException();
            base.bookState = new DraftBookState(base.book);
        }

        public override State State => State.Proposal;
    }
}