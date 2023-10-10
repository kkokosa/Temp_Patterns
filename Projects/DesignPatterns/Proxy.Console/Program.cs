var doc = new Document()
{
    Title = "My Document",
    Authors = new List<Author>()
    {
        new Author("John", "Doe"),
        new Author("Jane", "Doe")
    }
};
Console.WriteLine($"Document {doc.Title} is loaded: {doc.IsLoaded}");
Console.WriteLine($"Document content: {doc.Content}");
Console.WriteLine($"Document {doc.Title} is loaded: {doc.IsLoaded}");

class Document
{
    private Lazy<Content> _content = new Lazy<Content>(DocumentFactory);

    private static Content DocumentFactory()
    {
        // TODO: Load document from file
        return new Content("Lorem ipsum...");
    }

    public string Title { get; init; }
    public List<Author> Authors { get; init; } = new List<Author>();

    public Content Content => _content.Value;

#if DEBUG
    public bool IsLoaded => _content.IsValueCreated;
#endif
}

class Content
{
    private string _content;
    public Content(string content)
    {
        _content = content;
    }
}


public class Author : IEquatable<Author>
{
    public string FirstName { get; }
    public string LastName { get; }

    public Author(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("First name and last name cannot be empty.");
        }
        FirstName = firstName;
        LastName = lastName;
    }

    public override bool Equals(object obj) => Equals(obj as Author);

    public bool Equals(Author other) =>
        other != null &&
        FirstName == other.FirstName &&
        LastName == other.LastName;

    public override int GetHashCode() => HashCode.Combine(FirstName, LastName);

    public override string ToString() => $"{FirstName} {LastName}";
}
