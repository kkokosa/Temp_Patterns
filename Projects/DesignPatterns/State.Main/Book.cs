
using System;
using System.Collections.Generic;
using State.Main.States;

namespace State.Main
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
            this.state = Main.State.Proposal;
            this.chapters = new Dictionary<int, Chapter>();
            this.bookState = new ProposalBookState(this);
        }

        public State State => state;

        public void ChangeState(State newState)
        {
            switch (newState)
            {
                case State.Proposal:
                    // We can't move back to proposal
                    throw new InvalidOperationException();
                case State.Draft:
                    if (this.State != State.Proposal)
                        throw new InvalidOperationException();
                    this.state = State.Draft;
                    this.bookState = new DraftBookState(this);
                    break;
                case State.Production:
                    if (this.State != State.Draft)
                        throw new InvalidOperationException();
                    this.state = State.Production;
                    break;
                case State.Final:
                    if (this.State != State.Production)
                        throw new InvalidOperationException();
                    this.state = State.Final;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }

        public void AddChapter(int index, string title) => bookState.AddChapter(index, title);

        public void ChangeMetadata(BookMetadata metadata) => bookState.ChangeMetadata(metadata);

        public void GenerateIndex() => bookState.GenerateIndex();

        public void GenerateTableOfContent() => bookState.GenerateTableOfContent();

        public void Print() => bookState.Print();

        // Internal to not leak the implementation detail outside this assembly
        protected internal abstract class BookStateBase
        {
            protected Book book;

            // Gateway methods for states
            protected Dictionary<int, Chapter> chapters => book.chapters;
            protected ref BookMetadata metadata => ref book.metadata;

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

        #region Alternative state transition handling

        private readonly List<(State From, State To)> legalStateTransitions = new List<(State, State)>()
        {
            (State.Proposal, State.Draft),
            (State.Draft, State.Production),
            (State.Production, State.Final)
        };

        public void ChangeState2(State newState)
        {
            bool isLegal = legalStateTransitions.Exists(x => x.From == state && x.To == newState);
            if (!isLegal)
                throw new InvalidOperationException();
            this.state = newState;
        }

        #endregion
    }
}
