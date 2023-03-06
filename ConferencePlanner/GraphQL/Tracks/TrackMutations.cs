using ConferencePlanner.Data;
using ConferencePlanner.Data.Entities;

namespace ConferencePlanner.GraphQL.Tracks
{
    [MutationType]
    public class TrackMutations
    {
        public async Task<AddTrackPayload> AddTrackAsync(AddTrackInput input, ApplicationDbContext context, CancellationToken cancellationToken)
        {
            var track = new Track { Name = input.Name };
            context.Tracks.Add(track);

            await context.SaveChangesAsync(cancellationToken);

            return new AddTrackPayload(track);
        }

        public async Task<RenameTrackPayload> RenameTrackAsync(RenameTrackInput input, ApplicationDbContext context, CancellationToken cancellationToken)
        {
            Track? track = await context.Tracks.FindAsync(input.Id);
            if (track == null)
            {
                track = new Track { Name = input.Name };
                context.Tracks.Add(track);
            }
            else
            {
                track.Name = input.Name;
            }

            await context.SaveChangesAsync(cancellationToken);

            return new RenameTrackPayload(track);
        }
    }
}