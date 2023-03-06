using ConferencePlanner.Data;
using ConferencePlanner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Attendees
{
    public class SessionAttendeeCheckIn
    {
        public SessionAttendeeCheckIn(int attendeeId, int sessionId)
        {
            AttendeeId = attendeeId;
            SessionId = sessionId;
        }

        [ID<Attendee>]
        public int AttendeeId { get; }

        [ID<Session>]
        public int SessionId { get; }
        
        public async Task<int> CheckInCountAsync(ApplicationDbContext context, CancellationToken cancellationToken) 
            => await context.Sessions
                .Where(session => session.Id == SessionId)
                .SelectMany(session => session.SessionAttendees)
                .CountAsync(cancellationToken);

        public Task<Attendee> GetAttendeeAsync(IAttendeeByIdDataLoader attendeeById, CancellationToken cancellationToken) 
            => attendeeById.LoadAsync(AttendeeId, cancellationToken);

        public Task<Session> GetSessionAsync(ISessionByIdDataLoader sessionById, CancellationToken cancellationToken)
            => sessionById.LoadAsync(SessionId, cancellationToken);
    }
}