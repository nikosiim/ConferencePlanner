namespace ConferencePlanner.GraphQL.Books
{
    [ExtendObjectType<Book>]
    public class CountryExtensions
    {
        [BindMember(nameof(Book.Image))]
        public string? GetImageUrl([Parent] Book book, HttpContext context)
        {
            if (book.Image is null)
            {
                return null;
            }

            var scheme = context.Request.Scheme;
            var host = context.Request.Host.Value;
            return $"{scheme}://{host}/images/{book.Image}";
        }
    }

    [ExtendObjectType<Author>]
    public class BookExtensions
    {
        public string NameAndTitle([Parent] Author author, [ScopedState("book")] Book book)
        {
            return $"{author.Name} - {book.Title}";
        }
    }
}