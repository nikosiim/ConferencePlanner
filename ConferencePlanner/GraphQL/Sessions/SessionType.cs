using ConferencePlanner.Data;
using ConferencePlanner.Data.Entities;
using Microsoft.EntityFrameworkCore;
using static ConferencePlanner.GraphQL.DataLoaders;

namespace ConferencePlanner.GraphQL.Sessions
{
    public class SessionType : ObjectType<Session>
    {
        protected override void Configure(IObjectTypeDescriptor<Session> descriptor)
        {
            // Not needed anymore in version 13, just add an annotation [NodeResolver] to the corresponding query.
            // Also remove [ID(nameof(Session))] annotation from input parameter, leave just "int id"
            //descriptor.ImplementsNode().IdField(t => t.Id).ResolveNode((ctx, id) => ctx.DataLoader<ISessionByIdDataLoader>().LoadAsync(id, ctx.RequestAborted)!);

            descriptor
                .Field(t => t.SessionSpeakers)
                .ResolveWith<Resolvers>(t => t.GetSpeakersAsync(default!, default!, default!, default))
                .UseServiceScope()
                .Name("speakers");

            descriptor
                .Field(t => t.SessionAttendees)
                .ResolveWith<Resolvers>(t => t.GetAttendeesAsync(default!, default!, default!, default))
                .UseServiceScope()
                .Name("attendees");

            descriptor
                .Field(t => t.Track)
                .ResolveWith<Resolvers>(t => t.GetTrackAsync(default!, default!, default));

            descriptor
                .Field(t => t.TrackId)
                .ID(nameof(Track));
        }

        private class Resolvers
        {
            public async Task<IEnumerable<Speaker>> GetSpeakersAsync(
                [Parent] Session session, [Service] IServiceProvider service, ISpeakerByIdDataLoader speakerById, CancellationToken cancellationToken)
            {
                using var scope = service.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                int[] speakerIds = await dbContext.Sessions
                    .Where(s => s.Id == session.Id)
                    .Include(s => s.SessionSpeakers)
                    .SelectMany(s => s.SessionSpeakers.Select(t => t.SpeakerId))
                    .ToArrayAsync(cancellationToken: cancellationToken);

                return await speakerById.LoadAsync(speakerIds, cancellationToken);
            }

            public async Task<IEnumerable<Attendee>> GetAttendeesAsync(
                [Parent] Session session, [Service] IServiceProvider service, IAttendeeByIdDataLoader attendeeById, CancellationToken cancellationToken)
            {
                using var scope = service.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                int[] attendeeIds = await dbContext.Sessions
                    .Where(s => s.Id == session.Id)
                    .Include(s => s.SessionAttendees)
                    .SelectMany(s => s.SessionAttendees.Select(t => t.AttendeeId))
                    .ToArrayAsync(cancellationToken: cancellationToken);

                return await attendeeById.LoadAsync(attendeeIds, cancellationToken);
            } 

            public async Task<Track?> GetTrackAsync([Parent] Session session, ITrackByIdDataLoader trackById, CancellationToken cancellationToken)
            {
                if (session.TrackId is null)
                    return null;

                return await trackById.LoadAsync(session.TrackId.Value, cancellationToken);
            }
        }
    }
}