using System;

namespace State.PreMain.States
{
    internal class FinalBookState : BookStateBase
    {
        public FinalBookState(Book book) : base(book)
        {
        }

        public override void AddChapter(int index, string title)
        {
            throw new InvalidOperationException();
        }

        public override void ChangeMetadata(BookMetadata metadata)
        {
            throw new InvalidOperationException();
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
            // TODO: Implement printing
        }
    }
}