using ConferencePlanner.Data;
using ConferencePlanner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Tracks
{
    [QueryType]
    public class TrackQueries
    {
        [UsePaging]
        public IQueryable<Track> GetTracks(ApplicationDbContext context) 
            => context.Tracks.OrderBy(t => t.Name);
        
        public Task<Track> GetTrackByNameAsync(string name, ApplicationDbContext context, CancellationToken cancellationToken) 
            => context.Tracks.FirstAsync(t => t.Name == name, cancellationToken: cancellationToken);
        
        public async Task<IEnumerable<Track>> GetTrackByNamesAsync(string[] names, ApplicationDbContext context, CancellationToken cancellationToken)
            => await context.Tracks.Where(t => names.Contains(t.Name)).ToListAsync(cancellationToken: cancellationToken);

        [NodeResolver]
        public Task<Track> GetTrackByIdAsync(int id, ITrackByIdDataLoader trackById, CancellationToken cancellationToken) 
            => trackById.LoadAsync(id, cancellationToken);

        public async Task<IEnumerable<Track>> GetTracksByIdAsync([ID<Track>] int[] ids, ITrackByIdDataLoader trackById, CancellationToken cancellationToken) 
            => await trackById.LoadAsync(ids, cancellationToken);
    }
}