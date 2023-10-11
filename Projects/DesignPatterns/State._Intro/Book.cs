using System;
using System.Collections.Generic;

namespace State.Intro
{
    /// <summary>
    /// Book can be in various states, making various operations possible or not:
    ///            | AddChapter | ChangeMetadata | GenerateIndex | GenerateTOC | Print
    /// -----------|------------+----------------+---------------+-------------+------
    /// Proposal   |     v      |       v        |               |             |
    /// Draft      |     v      |       v        |               |             |
    /// Production |            |       v        |      v        |     v       |
    /// Final      |            |                |               |             |   v
    /// </summary>
    public class Book
    {
        private BookMetadata metadata;
        private readonly Dictionary<int, Chapter> chapters;
        private State state;

        public Book(BookMetadata metadata)
        {
            this.metadata = metadata;
            this.state = Intro.State.Proposal;
            this.chapters = new Dictionary<int, Chapter>();
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

        public void AddChapter(int index, string title)
        {
            if (this.State == State.Final || this.State == State.Production)
                throw new InvalidOperationException();
            this.chapters.Add(index, new Chapter(title));
        }

        public void ChangeMetadata(BookMetadata metadata)
        {
            if (this.State == State.Final)
                throw new InvalidOperationException();
            this.metadata = metadata;
        }

        public void GenerateIndex()
        {
            if (this.State == State.Proposal ||
                this.State == State.Draft || 
                this.State == State.Final)
                throw new InvalidOperationException();
        }

        public void GenerateTableOfContent()
        {
            if (this.State == State.Proposal ||
                this.State == State.Draft ||
                this.State == State.Final)
                throw new InvalidOperationException();

        }

        public void Print()
        {
            if (this.State != State.Final)
                throw new InvalidOperationException();
        }
    }
}
