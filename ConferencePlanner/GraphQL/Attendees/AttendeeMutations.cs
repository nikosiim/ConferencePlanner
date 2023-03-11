﻿using ConferencePlanner.Data;
using ConferencePlanner.Data.Entities;
using ConferencePlanner.GraphQL.Common;
using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Attendees
{
    [MutationType]
    public class AttendeeMutations
    {
        public async Task<RegisterAttendeePayload> RegisterAttendeeAsync(RegisterAttendeeInput input, ApplicationDbContext context, CancellationToken cancellationToken)
        {
            Attendee? attendee = await context.Attendees.FirstOrDefaultAsync(t => t.EmailAddress == input.EmailAddress, cancellationToken);
            if (attendee is null)
            {
                attendee = new Attendee
                {
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    UserName = input.UserName,
                    EmailAddress = input.EmailAddress
                };
                context.Attendees.Add(attendee);
                await context.SaveChangesAsync(cancellationToken);
            }

            return new RegisterAttendeePayload(attendee);
        }

        public async Task<CheckInAttendeePayload> CheckInAttendeeAsync(
            CheckInAttendeeInput input, 
            ApplicationDbContext context, 
            [Service] ITopicEventSender eventSender,
            CancellationToken cancellationToken)
        {
            Attendee? attendee = await context.Attendees.FirstOrDefaultAsync(t => t.Id == input.AttendeeId, cancellationToken);
            if (attendee is null)
            {
                return new CheckInAttendeePayload(new UserError("Attendee not found.", "ATTENDEE_NOT_FOUND"));
            }

            try
            {
                attendee.SessionsAttendees.Add(new SessionAttendee { SessionId = input.SessionId });
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            await eventSender.SendAsync("OnAttendeeCheckedIn_" + input.SessionId, input.AttendeeId, cancellationToken);

            return new CheckInAttendeePayload(attendee, input.SessionId);
        }
    }
}