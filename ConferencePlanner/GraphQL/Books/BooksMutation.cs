using HotChocolate.Subscriptions;

namespace ConferencePlanner.GraphQL.Books
{
    [MutationType]
    public class BooksMutation
    {
        public async Task<Book> PublishBook(string title, [Service] ITopicEventSender eventSender, CancellationToken cancellationToken)
        {
            var book = new Book { Title = title };
            await eventSender.SendAsync(nameof(PublishBook), book, cancellationToken);

            return book;
        }
    }
}