using ConferencePlanner.Data;
using ConferencePlanner.Data.Entities;

namespace ConferencePlanner.GraphQL.Attendees
{
    [QueryType]
    public class AttendeeQueries
    {
        [UsePaging]
        public IQueryable<Attendee> GetAttendees(ApplicationDbContext context) =>
            context.Attendees;

        [NodeResolver]
        public Task<Attendee> GetAttendeeByIdAsync(int id, IAttendeeByIdDataLoader attendeeById, CancellationToken cancellationToken)
            => attendeeById.LoadAsync(id, cancellationToken);

        public async Task<IEnumerable<Attendee>> GetAttendeesByIdAsync([ID(nameof(Attendee))] int[] ids, IAttendeeByIdDataLoader attendeeById, CancellationToken cancellationToken)
            => await attendeeById.LoadAsync(ids, cancellationToken);
    }
}