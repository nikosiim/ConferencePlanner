using ConferencePlanner.Data.Entities;

namespace ConferencePlanner.GraphQL.Tracks
{
    public record RenameTrackInput([property: ID<Track>] int Id, string Name);
}