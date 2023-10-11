namespace State.Intro.Tests;

public static class BookAssembler
{
    public static Book AssembleWithState(State state)
    {
        var book = new Book(new BookMetadata("Test book", "Joe Doe", "book, great, programming"));
        switch (state)
        {
            case State.Draft:
                book.ChangeState(State.Draft);
                break;
            case State.Production:
                book.ChangeState(State.Draft);
                book.ChangeState(State.Production);
                break;
            case State.Final:
                book.ChangeState(State.Draft);
                book.ChangeState(State.Production);
                book.ChangeState(State.Final);
                break;
        }
        return book;
    }
}