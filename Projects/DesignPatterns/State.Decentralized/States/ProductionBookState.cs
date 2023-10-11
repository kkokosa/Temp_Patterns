using System;
using State.Decentralized.Exceptions;

namespace State.Decentralized.States
{
    internal class ProductionBookState : Book.BookStateBase
    {
        public ProductionBookState(Book book) : base(book)
        {
        }

        public override void AddChapter(int index, string title)
        {
            throw new InvalidBookOperationException(nameof(AddChapter), nameof(ProductionBookState));
        }

        public override void ChangeMetadata(BookMetadata metadata)
        {
            base.metadata = metadata;
        }

        public override void GenerateIndex()
        {
            // TODO: Implement generating index
        }

        public override void GenerateTableOfContent()
        {
            // TODO: Implement generating TOC
        }

        public override void Print()
        {
            throw new InvalidBookOperationException(nameof(Print), nameof(ProductionBookState));
        }

        public override void MoveToState(State state)
        {
            if (state != State.Final)
                throw new InvalidOperationException();
            base.bookState = new FinalBookState(book);
        }

        public override State State => State.Production;
    }
}