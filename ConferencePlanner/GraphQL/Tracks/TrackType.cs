using ConferencePlanner.Data;
using ConferencePlanner.Data.Entities;
using ConferencePlanner.GraphQL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Tracks
{
    public class TrackType : ObjectType<Track>
    {
        protected override void Configure(IObjectTypeDescriptor<Track> descriptor)
        {
            descriptor.Field(t => t.Sessions)
                .ResolveWith<Resolvers>(t => t.GetSessionsAsync(default!, default!, default!, default))
                .UseServiceScope()
                .Name("sessions");

            descriptor.Field(t => t.Name).UseUpperCase();
        }

        private class Resolvers
        {
            public async Task<IEnumerable<Session>> GetSessionsAsync(
                [Parent] Track track, [Service] IServiceProvider service, ISessionByIdDataLoader sessionById, CancellationToken cancellationToken)
            {
                using var scope = service.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                int[] sessionIds = await dbContext.Sessions
                    .Where(s => s.Id == track.Id)
                    .Select(s => s.Id)
                    .ToArrayAsync(cancellationToken: cancellationToken);

                return await sessionById.LoadAsync(sessionIds, cancellationToken);
            }
        }
    }
}