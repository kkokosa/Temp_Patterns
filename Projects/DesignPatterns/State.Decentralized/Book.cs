using System;
using System.Collections.Generic;

namespace State.Decentralized
{
    public class Book
    {
        private BookMetadata metadata;
        private Dictionary<int, Chapter> chapters;
        private State state;
        private BookStateBase bookState;

        public Book(BookMetadata metadata)
        {
            this.metadata = metadata;
            this.state = State.Proposal;
            this.chapters = new Dictionary<int, Chapter>();
        }

        public State State => bookState.State;

        public void AddChapter(int index, string title) => bookState.AddChapter(index, title);

        public void ChangeMetadata(BookMetadata metadata) => bookState.ChangeMetadata(metadata);

        public void GenerateIndex() => bookState.GenerateIndex();

        public void GenerateTableOfContent() => bookState.GenerateTableOfContent();

        public void Print() => bookState.Print();

        public void MoveToState(State state) => bookState.MoveToState(state);

        // Internal to not leak the implementation detail outside this assembly
        protected internal abstract class BookStateBase
        {
            protected Book book;

            // Gateway methods for states
            protected Dictionary<int, Chapter> chapters => book.chapters;
            protected ref BookMetadata metadata => ref book.metadata;
            protected BookStateBase bookState
            {
                get => book.bookState;
                set => book.bookState = value;
            }

            protected BookStateBase(Book book)
            {
                this.book = book;
            }

            public abstract void AddChapter(int index, string title);
            public abstract void ChangeMetadata(BookMetadata metadata);
            public abstract void GenerateIndex();
            public abstract void GenerateTableOfContent();
            public abstract void Print();
            public abstract void MoveToState(State state);
            public abstract State State { get; }
        }
    }
}
