using ConferencePlanner.Data;
using ConferencePlanner.Data.Entities;

namespace ConferencePlanner.GraphQL.Speakers
{
    [QueryType]
    public class SpeakerQueries
    {
        [UsePaging]
        public IQueryable<Speaker> GetSpeakers(ApplicationDbContext context) =>
            context.Speakers.OrderBy(t => t.Name);

        [NodeResolver]
        public Task<Speaker> GetSpeakerByIdAsync(int id, ISpeakerByIdDataLoader dataLoader, CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);

        public async Task<IEnumerable<Speaker>> GetSpeakersByIdAsync([ID(nameof(Speaker))]int[] ids, ISpeakerByIdDataLoader dataLoader, CancellationToken cancellationToken) =>
            await dataLoader.LoadAsync(ids, cancellationToken);
    }
}