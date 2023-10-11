using System;
using System.Runtime.Serialization;
using State.Main.Exceptions;

namespace State.Main.States
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
    }
}
