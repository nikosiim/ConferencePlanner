using ConferencePlanner.Data;
using ConferencePlanner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Speakers
{
    public class SpeakerType : ObjectType<Speaker>
    {
        protected override void Configure(IObjectTypeDescriptor<Speaker> descriptor)
        {
            descriptor
                .Field(t => t.SessionSpeakers)
                .ResolveWith<Resolvers>(t => t.GetSessionsAsync(default!, default!, default!, default))
                .UseServiceScope()
                .Name("sessions");
        }

        private class Resolvers
        {
            public async Task<IEnumerable<Session>> GetSessionsAsync(
                [Parent] Speaker speaker, [Service] IServiceProvider service, ISessionByIdDataLoader sessionById, CancellationToken cancellationToken)
            {
                using var scope = service.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                int[] sessionIds = await dbContext.Speakers
                    .Where(s => s.Id == speaker.Id)
                    .Include(s => s.SessionSpeakers)
                    .SelectMany(s => s.SessionSpeakers.Select(t => t.SessionId))
                    .ToArrayAsync(cancellationToken: cancellationToken);

                return await sessionById.LoadAsync(sessionIds, cancellationToken);
            }
        }
    }
}