using System.Runtime.CompilerServices;
using HotChocolate.Subscriptions;

namespace ConferencePlanner.GraphQL.Books
{
    [SubscriptionType]
    public class BooksSubscriptions
    {
        public async IAsyncEnumerable<Book> OnPublishedStream([Service] ITopicEventReceiver eventReceiver, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var sourceStream = await eventReceiver.SubscribeAsync<Book>(nameof(BooksMutation.PublishBook), cancellationToken);

            yield return new Book { Title = "First book" };
            await Task.Delay(5000, cancellationToken);

            await foreach (Book book in sourceStream.ReadEventsAsync().WithCancellation(cancellationToken))
            {
                yield return book;
            }
        }

        [Subscribe(With = nameof(OnPublishedStream))]
        public Book OnPublished([EventMessage] Book publishedBook) => publishedBook;
    }
}