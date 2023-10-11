using System;
using State.Main.Exceptions;

namespace State.Main.States
{
    //internal class DraftBookState : IBookState
    //{
    //    DraftBookState(Book book)
    //    {

    //    }

    //    void AddChapter()
    //    {
    //        //...
    //        book.AddChapter
    //    }
    //}

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
    }
}