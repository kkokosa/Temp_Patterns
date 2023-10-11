using System;
using System.Collections.Generic;
using State.PreMain.States;

namespace State.PreMain
{
    public partial class Book
    {
        private BookMetadata metadata;
        private Dictionary<int, Chapter> chapters;
        private State state;
        private BookStateBase bookState;

        public Book(BookMetadata metadata)
        {
            this.metadata = metadata;
            this.state = State.Proposal;
            this.bookState = new ProposalBookState(this);
            this.chapters = new Dictionary<int, Chapter>();
        }

        public State State => state;

        public Dictionary<int, Chapter> Chapters => chapters;
        public BookMetadata Metadata
        {
            get => metadata;
            set => metadata = value;
        }

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
                    this.bookState = new ProductionBookState(this);
                    break;
                case State.Final:
                    if (this.State != State.Production)
                        throw new InvalidOperationException();
                    this.state = State.Final;
                    this.bookState = new FinalBookState(this);
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
    }
}
