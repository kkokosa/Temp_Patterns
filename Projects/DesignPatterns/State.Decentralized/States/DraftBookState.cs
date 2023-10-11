using System;
using State.Decentralized.Exceptions;

namespace State.Decentralized.States
{
    internal class DraftBookState : Book.BookStateBase
    {
        public DraftBookState(Book book) : base(book)
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
            throw new InvalidBookOperationException(nameof(GenerateIndex), nameof(DraftBookState));
        }

        public override void GenerateTableOfContent()
        {
            throw new InvalidBookOperationException(nameof(GenerateTableOfContent), nameof(DraftBookState));
        }

        public override void Print()
        {
            throw new InvalidBookOperationException(nameof(Print), nameof(DraftBookState));
        }

        public override void MoveToState(State state)
        {
            if (state != State.Production)    
                throw new InvalidOperationException();
            base.bookState = new ProposalBookState(base.book);
        }

        public override State State => State.Draft;
    }
}
