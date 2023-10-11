using System;
using Xunit;

namespace State.Intro.Tests
{
    public class SimpleStateIntroTests
    {
        [Fact]
        public void Given_InitialBook_When_CheckingState_Then_ShouldBeProposal()
        {
            Book book = new Book(new BookMetadata());
            
            Assert.Equal(State.Proposal, book.State);
        }

        [Fact]
        public void Given_InitialBook_When_StartingDraft_Then_ShouldSucceed()
        {
            Book book = new Book(new BookMetadata());

            book.ChangeState(State.Draft);

            Assert.Equal(State.Draft, book.State);
        }

        [Fact]
        public void Given_InitialBook_When_StartingProduction_Then_ShouldThrow()
        {
            Book book = new Book(new BookMetadata());

            var exception = Record.Exception(() => book.ChangeState(State.Production));

            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public void Given_InitialBook_When_Printing_Then_ShouldThrow()
        {
            Book book = new Book(new BookMetadata());

            var exception = 
                Record.Exception(() => book.Print());

            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }
    }
}
