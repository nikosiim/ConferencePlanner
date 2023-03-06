using ConferencePlanner.Data;
using ConferencePlanner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL
{
    public static class DataLoaders
    {
        [DataLoader]
        internal static async Task<IReadOnlyDictionary<int, Speaker>> SpeakerByIdDataLoader (
            IReadOnlyList<int> keys, 
            ApplicationDbContext context, 
            CancellationToken cancellationToken)
        {
            return await context.Speakers.Where(s => keys.Contains(s.Id)).ToDictionaryAsync(t => t.Id, cancellationToken);
        }

        [DataLoader]
        internal static async Task<IReadOnlyDictionary<int, Session>> SessionByIdDataLoader (
            IReadOnlyList<int> keys, 
            ApplicationDbContext context, 
            CancellationToken cancellationToken)
        {
            return await context.Sessions.Where(s => keys.Contains(s.Id)).ToDictionaryAsync(t => t.Id, cancellationToken);
        }

        [DataLoader]
        internal static async Task<IReadOnlyDictionary<int, Attendee>> AttendeeByIdDataLoader (
            IReadOnlyList<int> keys, 
            ApplicationDbContext context, 
            CancellationToken cancellationToken)
        {
            return await context.Attendees.Where(s => keys.Contains(s.Id)).ToDictionaryAsync(t => t.Id, cancellationToken);
        }

        [DataLoader]
        internal static async Task<IReadOnlyDictionary<int, Track>> TrackByIdDataLoader (
            IReadOnlyList<int> keys, 
            ApplicationDbContext context, 
            CancellationToken cancellationToken)
        {
            return await context.Tracks.Where(s => keys.Contains(s.Id)).ToDictionaryAsync(t => t.Id, cancellationToken);
        }
    }
}