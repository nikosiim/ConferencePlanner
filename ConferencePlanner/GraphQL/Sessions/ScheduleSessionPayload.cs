using ConferencePlanner.Data.Entities;
using ConferencePlanner.Data;
using ConferencePlanner.GraphQL.Common;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Sessions
{
    public class ScheduleSessionPayload : SessionPayloadBase
    {
        public ScheduleSessionPayload(Session session)
            : base(session)
        {
        }

        public ScheduleSessionPayload(UserError error)
            : base(new[] { error })
        {
        }

        public async Task<Track?> GetTrackAsync(ITrackByIdDataLoader trackById, CancellationToken cancellationToken)
        {
            if (Session is null)
                return null;

            return await trackById.LoadAsync(Session.Id, cancellationToken);
        }

        // Todo: add NodeResolver attribute
        public async Task<IEnumerable<Speaker>?> GetSpeakersAsync(ApplicationDbContext dbContext, ISpeakerByIdDataLoader speakerById, CancellationToken cancellationToken)
        {
            if (Session is null)
                return null;

            int[] speakerIds = await dbContext.Sessions
                .Where(s => s.Id == Session.Id)
                .Include(s => s.SessionSpeakers)
                .SelectMany(s => s.SessionSpeakers.Select(t => t.SpeakerId))
                .ToArrayAsync(cancellationToken: cancellationToken);

            return await speakerById.LoadAsync(speakerIds, cancellationToken);
        }
    }
}