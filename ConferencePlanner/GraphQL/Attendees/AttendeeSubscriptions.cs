using ConferencePlanner.Data.Entities;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;

namespace ConferencePlanner.GraphQL.Attendees
{
    [SubscriptionType]
    public class AttendeeSubscriptions
    {
        [Subscribe(With = nameof(SubscribeToOnAttendeeCheckedInAsync))]
        public SessionAttendeeCheckIn OnAttendeeCheckedIn([ID<Session>] int sessionId, [EventMessage] int attendeeId) 
            => new SessionAttendeeCheckIn(attendeeId, sessionId);

        public async ValueTask<ISourceStream<string>> SubscribeToOnAttendeeCheckedInAsync(int sessionId, [Service] ITopicEventReceiver eventReceiver, CancellationToken cancellationToken) 
            => await eventReceiver.SubscribeAsync<string>("OnAttendeeCheckedIn_" + sessionId, cancellationToken);
    }
}