using ConferencePlanner.Data;
using ConferencePlanner.Data.Entities;

namespace ConferencePlanner.GraphQL.Sessions
{
    [QueryType]
    public class SessionQueries
    {
        // It is important that a connection type works with a fixed item type if we mix attribute and fluent syntax.???

        [UsePaging(typeof(NonNullType<SessionType>))]
        [UseFiltering<SessionFilterInputType>]
        [UseSorting]
        public IQueryable<Session> GetSessions(ApplicationDbContext context) 
            => context.Sessions;

        [NodeResolver]
        public Task<Session> GetSessionByIdAsync(int id, ISessionByIdDataLoader sessionById, CancellationToken cancellationToken) 
            => sessionById.LoadAsync(id, cancellationToken);
        
        public async Task<IEnumerable<Session>> GetSessionsByIdAsync([ID<Session>] int[] ids, ISessionByIdDataLoader sessionById, CancellationToken cancellationToken) 
            => await sessionById.LoadAsync(ids, cancellationToken);
    }
}