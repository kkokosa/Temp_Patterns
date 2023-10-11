using System.Collections.Generic;

namespace State.PreMain
{
    internal abstract class BookStateBase
    {
        protected Book book;

        protected BookStateBase(Book book)
        {
            this.book = book;
        }

        public abstract void AddChapter(int index, string title);
        public abstract void ChangeMetadata(BookMetadata metadata);
        public abstract void GenerateIndex();
        public abstract void GenerateTableOfContent();
        public abstract void Print();
    }
}