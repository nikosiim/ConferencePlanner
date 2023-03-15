using ConferencePlanner.Data;
using ConferencePlanner.Data.Entities;
using ConferencePlanner.GraphQL.Extensions;
using ConferencePlanner.GraphQL.Sessions;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Tracks
{
    public class TrackType : ObjectType<Track>
    {
        /*
            NOTES:
            
            1. There is one caveat in our implementation with the TrackType. 
                Since, we are using a DataLoader within our resolver and first fetch the list of IDs we essentially will always fetch everything and chop in memory. 
                In an actual project this can be split into two actions by moving the DataLoader part into a middleware and first page on the id queryable. 
                Also one could implement a special IPagingHandler that uses the DataLoader and applies paging logic.

            2. Exposing rich filters to a public API can lead to unpredictable performance implications, but using filters wisely on select fields can make your 
                API much better to use. In conference API it would make almost no sense to expose filters on top of the tracks field since the Track type really 
                only has one field name and filtering on that really seems overkill. 
        */

        protected override void Configure(IObjectTypeDescriptor<Track> descriptor)
        {
            /*
            // this defines that fields shall only be defined explicitly
            descriptor.BindFieldsExplicitly();

            // now declare the fields that you want to define.
            descriptor.Field(t => t.Name);  
            */

            descriptor.Field(t => t.Sessions)
                .ResolveWith<Resolvers>(t => t.GetSessionsAsync(default!, default!, default!, default))
                .UseServiceScope()
                .UsePaging<NonNullType<SessionType>>()
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