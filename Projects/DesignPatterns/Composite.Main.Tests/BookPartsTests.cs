namespace Composite.Main.Tests
{
    public class BookPartsTests
    {
        [Fact]
        public void Test1()
        {
            var book = new Chapter("Book", 0, 12);
            book.Add(new Paragraph(13, 600));
            book.Add(new Figure(@"images/figure1.png", 601, 615));
            var chapter1 = new Chapter("Chapter 1", 616, 624);
            chapter1.Add(new Paragraph(624, 1800));
        }
    }
}