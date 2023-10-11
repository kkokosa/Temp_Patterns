using System;
using FluentAssertions;
using Xunit;

namespace State.Intro.Tests
{
    public class StateTransitionTests
    {
        [Fact]
        public void Given_InitialBook_When_CheckingState_Then_ShouldBeProposal()
        {
            var book = new Book(new BookMetadata("Test book", "Joe Doe", "book, great, programming"));
            
            book.State.Should().Be(State.Proposal);
        }

        /// Proposal --> Draft --> Production --> Final
        ///                ^          |
        ///                |          |
        ///                +----------+
        [Theory]
        [InlineData(State.Proposal, State.Draft, true)]
        [InlineData(State.Proposal, State.Production, false)]
        [InlineData(State.Proposal, State.Final, false)]
        [InlineData(State.Draft, State.Proposal, false)]
        [InlineData(State.Draft, State.Production, true)]
        [InlineData(State.Draft, State.Final, false)]
        [InlineData(State.Production, State.Proposal, false)]
        [InlineData(State.Production, State.Draft, true)]
        [InlineData(State.Production, State.Final, true)]
        [InlineData(State.Final, State.Proposal, false)]
        [InlineData(State.Final, State.Draft, false)]
        [InlineData(State.Final, State.Production, false)]
        public void Given_Book_When_ChangeState_Then_ShouldBehaveAsExpected(State from, State to, bool isAllowed)
        {
            var book = BookAssembler.AssembleWithState(from);

            var ex = Record.Exception(() => book.ChangeState(to));

            if (isAllowed)
            {
                ex.Should().BeNull();
                book.State.Should().Be(to);
            }
            else
            {
                ex.Should().NotBeNull()
                    .And.BeOfType<InvalidOperationException>();
                book.State.Should().Be(from);
            }
        }
    }
}
