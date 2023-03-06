using ConferencePlanner.Data.Entities;
using ConferencePlanner.Data;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Speakers
{
    [QueryType]
    public class SpeakerQueries
    {
        public Task<List<Speaker>> GetSpeakers(ApplicationDbContext context) =>
            context.Speakers.ToListAsync();

        [NodeResolver]
        public Task<Speaker> GetSpeakerByIdAsync(int id, ISpeakerByIdDataLoader dataLoader, CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);

        public async Task<IEnumerable<Speaker>> GetSpeakersByIdAsync([ID(nameof(Speaker))]int[] ids, ISpeakerByIdDataLoader dataLoader, CancellationToken cancellationToken) =>
            await dataLoader.LoadAsync(ids, cancellationToken);
    }
}