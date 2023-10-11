using State.Decentralized.Exceptions;

namespace State.Decentralized.States
{
    internal class FinalBookState : Book.BookStateBase
    {
        public FinalBookState(Book book) : base(book)
        {
        }

        public override void AddChapter(int index, string title)
        {
            throw new InvalidBookOperationException(nameof(AddChapter), nameof(FinalBookState));
        }

        public override void ChangeMetadata(BookMetadata metadata)
        {
            throw new InvalidBookOperationException(nameof(ChangeMetadata), nameof(FinalBookState));
        }

        public override void GenerateIndex()
        {
            throw new InvalidBookOperationException(nameof(GenerateIndex), nameof(FinalBookState));
        }

        public override void GenerateTableOfContent()
        {
            throw new InvalidBookOperationException(nameof(GenerateTableOfContent), nameof(FinalBookState));
        }

        public override void Print()
        {
            // TODO: Implement printing
        }

        public override void MoveToState(State state)
        {
            throw new System.NotImplementedException();
        }

        public override State State => State.Final;
    }
}