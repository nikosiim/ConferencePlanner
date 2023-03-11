using ConferencePlanner.Data.Entities;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;

namespace ConferencePlanner.GraphQL.Attendees
{
    [SubscriptionType]
    public class AttendeeSubscriptions
    {
       
        public async ValueTask<ISourceStream<int>> SubscribeToOnAttendeeCheckedInAsync(
            int sessionId,
            [Service] ITopicEventReceiver eventReceiver,
            CancellationToken cancellationToken)
        {
            string topicName = "OnAttendeeCheckedIn_" + sessionId;
            return await eventReceiver.SubscribeAsync<int>(topicName, cancellationToken);
        }

        [Subscribe(With = nameof(SubscribeToOnAttendeeCheckedInAsync))]
        public SessionAttendeeCheckIn OnAttendeeCheckedIn([ID<Session>] int sessionId, [EventMessage] int attendeeId) 
            => new SessionAttendeeCheckIn(attendeeId, sessionId);
    }
}