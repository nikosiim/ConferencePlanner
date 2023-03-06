using ConferencePlanner.Data.Entities;

namespace ConferencePlanner.GraphQL.Attendees
{
    public record CheckInAttendeeInput([ID(nameof(Session))] int SessionId, [ID(nameof(Attendee))] int AttendeeId);
}