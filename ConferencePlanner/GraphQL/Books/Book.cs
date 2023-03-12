namespace ConferencePlanner.GraphQL.Books
{
    public class Book
    {
        public required string Title { get; set; }
        public Author? Author { get; set; }
    }
}