using ConferencePlanner.Data.Entities;
using ConferencePlanner.GraphQL.Common;

namespace ConferencePlanner.GraphQL.Attendees
{
    public class RegisterAttendeePayload : AttendeePayloadBase
    {
        public RegisterAttendeePayload(Attendee attendee)
            : base(attendee)
        {
        }

        public RegisterAttendeePayload(UserError error)
            : base(new[] { error })
        {
        }
    }
}