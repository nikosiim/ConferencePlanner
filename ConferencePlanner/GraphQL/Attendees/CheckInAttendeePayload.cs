using ConferencePlanner.Data.Entities;
using ConferencePlanner.GraphQL.Common;

namespace ConferencePlanner.GraphQL.Attendees
{
    public class CheckInAttendeePayload : AttendeePayloadBase
    {
        private readonly int? _sessionId;

        public CheckInAttendeePayload(Attendee attendee, int sessionId)
            : base(attendee)
        {
            _sessionId = sessionId;
        }

        public CheckInAttendeePayload(UserError error)
            : base(new[] { error })
        {
        }

        public async Task<Session?> GetSessionAsync(ISessionByIdDataLoader sessionById, CancellationToken cancellationToken)
        {
            if (_sessionId.HasValue)
            {
                return await sessionById.LoadAsync(_sessionId.Value, cancellationToken);
            }

            return null;
        }
    }
}