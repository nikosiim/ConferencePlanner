using ConferencePlanner.Data.Entities;

namespace ConferencePlanner.GraphQL.Attendees
{
    public record CheckInAttendeeInput([property: ID<Session>] int SessionId, [property: ID<Attendee>] int AttendeeId);
}