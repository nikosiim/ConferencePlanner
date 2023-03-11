using ConferencePlanner.Data.Entities;

namespace ConferencePlanner.GraphQL.Sessions
{
    [SubscriptionType]
    public class SessionSubscriptions
    {
        [Subscribe]
        [Topic]
        public Task<Session> OnSessionScheduled([EventMessage] int sessionId, ISessionByIdDataLoader sessionById, CancellationToken cancellationToken) 
            => sessionById.LoadAsync(sessionId, cancellationToken);
    }
}