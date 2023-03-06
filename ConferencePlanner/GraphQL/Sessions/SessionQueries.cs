using ConferencePlanner.Data;
using ConferencePlanner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Sessions
{
    [QueryType]
    public class SessionQueries
    {
        public async Task<IEnumerable<Session>> GetSessionsAsync(ApplicationDbContext context, CancellationToken cancellationToken) 
            => await context.Sessions.ToListAsync(cancellationToken);

        [NodeResolver]
        public Task<Session> GetSessionByIdAsync(int id, ISessionByIdDataLoader sessionById, CancellationToken cancellationToken) 
            => sessionById.LoadAsync(id, cancellationToken);
        
        public async Task<IEnumerable<Session>> GetSessionsByIdAsync([ID<Session>] int[] ids, ISessionByIdDataLoader sessionById, CancellationToken cancellationToken) 
            => await sessionById.LoadAsync(ids, cancellationToken);
    }
}