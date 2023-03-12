namespace ConferencePlanner.GraphQL.Books
{
    [ExtendObjectType<Author>]
    public class BookExtensions
    {
        public string NameAndTitle([Parent] Author author, [ScopedState("book")] Book book)
        {
            return $"{author.Name} - {book.Title}";
        }
    }
}