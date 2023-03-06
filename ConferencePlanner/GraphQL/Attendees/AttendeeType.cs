using ConferencePlanner.Data.Entities;
using ConferencePlanner.Data;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Attendees
{
    public class AttendeeType : ObjectType<Attendee>
    {
        protected override void Configure(IObjectTypeDescriptor<Attendee> descriptor)
        {
            descriptor
                .Field(t => t.SessionsAttendees)
                .ResolveWith<AttendeeResolvers>(t => t.GetSessionsAsync(default!, default!, default!, default))
                .UseDbContext<ApplicationDbContext>()
                .Name("sessions");
        }

        private class AttendeeResolvers
        {
            public async Task<IEnumerable<Session>> GetSessionsAsync(
                [Parent] Attendee attendee, [Service] IServiceProvider service, SessionByIdDataLoader sessionById, CancellationToken cancellationToken)
            {
                using var scope = service.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                
                int[] speakerIds = await dbContext.Attendees
                    .Where(a => a.Id == attendee.Id)
                    .Include(a => a.SessionsAttendees)
                    .SelectMany(a => a.SessionsAttendees.Select(t => t.SessionId))
                    .ToArrayAsync(cancellationToken: cancellationToken);

                return await sessionById.LoadAsync(speakerIds, cancellationToken);
            }
        }
    }
}