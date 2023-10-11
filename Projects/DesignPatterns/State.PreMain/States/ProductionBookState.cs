using System;

namespace State.PreMain.States
{
    internal class ProductionBookState : BookStateBase
    {
        public ProductionBookState(Book book) : base(book)
        {
        }

        public override void AddChapter(int index, string title)
        {
            throw new InvalidOperationException();
        }

        public override void ChangeMetadata(BookMetadata metadata)
        {
            book.Metadata = metadata;
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
            throw new InvalidOperationException();
        }
    }
}