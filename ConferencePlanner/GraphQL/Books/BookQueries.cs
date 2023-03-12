using ConferencePlanner.GraphQL.Extensions;

namespace ConferencePlanner.GraphQL.Books
{
    [QueryType]
    public class BookQueries
    {
        [PreserveParentAs("book")]
        public Book GetBook() => new Book { Title = "C# in depth", Author = new Author { Name = "Jon Skeet" } };
    }
}