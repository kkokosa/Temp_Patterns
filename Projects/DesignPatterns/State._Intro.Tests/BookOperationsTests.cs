using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace State.Intro.Tests
{
    public class BookOperationsTests
    {
        ///            | AddChapter | ChangeMetadata | GenerateIndex | GenerateTOC | Print
        /// -----------|------------+----------------+---------------+-------------+------
        /// Proposal   |     v      |       v        |               |             |
        /// Draft      |     v      |       v        |      v        |     v       |
        /// Production |            |       v        |      v        |     v       |   
        /// Final      |

        [Theory]
        [InlineData(State.Proposal, true)]
        [InlineData(State.Draft, true)]
        [InlineData(State.Production, false)]
        [InlineData(State.Final, false)]
        public void Given_Book_When_AddingChapter_Then_ShouldBehaveAsExpected(State state, bool isAllowed)
        {
            var book = BookAssembler.AssembleWithState(state);

            var exception = Record.Exception(
                () => book.AddChapter(0, "Lorem ipsum"));

            if (isAllowed)
                exception.Should().BeNull();
            else
                exception.Should().NotBeNull()
                .And.BeOfType<InvalidOperationException>();
        }

        [Theory]
        [InlineData(State.Proposal, true)]
        [InlineData(State.Draft, true)]
        [InlineData(State.Production, true)]
        [InlineData(State.Final, false)]
        public void Given_Book_When_ChangingMetadata_Then_ShouldBehaveAsExpected(State state, bool isAllowed)
        {
            var book = BookAssembler.AssembleWithState(state);

            var exception = Record.Exception(
                () => book.ChangeMetadata(new BookMetadata("Test book", "Joe Doe", "book, great, programming")));

            if (isAllowed)
                exception.Should().BeNull();
            else
                exception.Should().NotBeNull()
                    .And.BeOfType<InvalidOperationException>();
        }

        [Theory]
        [InlineData(State.Proposal, false)]
        [InlineData(State.Draft, true)]
        [InlineData(State.Production, true)]
        [InlineData(State.Final, false)]
        public void Given_Book_When_GeneratingIndex_Then_ShouldBehaveAsExpected(State state, bool isAllowed)
        {
            var book = BookAssembler.AssembleWithState(state);

            var exception = Record.Exception(
                () => book.GenerateIndex());

            if (isAllowed)
                exception.Should().BeNull();
            else
                exception.Should().NotBeNull()
                    .And.BeOfType<InvalidOperationException>();
        }

        [Theory]
        [InlineData(State.Proposal, false)]
        [InlineData(State.Draft, true)]
        [InlineData(State.Production, true)]
        [InlineData(State.Final, false)]
        public void Given_Book_When_GeneratingToc_Then_ShouldBehaveAsExpected(State state, bool isAllowed)
        {
            var book = BookAssembler.AssembleWithState(state);

            var exception = Record.Exception(
                () => book.GenerateTableOfContent());

            if (isAllowed)
                exception.Should().BeNull();
            else
                exception.Should().NotBeNull()
                    .And.BeOfType<InvalidOperationException>();
        }

        [Theory]
        [InlineData(State.Proposal, false)]
        [InlineData(State.Draft, false)]
        [InlineData(State.Production, false)]
        [InlineData(State.Final, true)]
        public void Given_Book_When_Printing_Then_ShouldBehaveAsExpected(State state, bool isAllowed)
        {
            var book = BookAssembler.AssembleWithState(state);

            var exception = Record.Exception(
                () => book.Print());

            if (isAllowed)
                exception.Should().BeNull();
            else
                exception.Should().NotBeNull()
                    .And.BeOfType<InvalidOperationException>();
        }
    }
}
