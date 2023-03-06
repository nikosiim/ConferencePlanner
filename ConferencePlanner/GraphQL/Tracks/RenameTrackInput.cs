using ConferencePlanner.Data.Entities;

namespace ConferencePlanner.GraphQL.Tracks
{
    public record RenameTrackInput([ID(nameof(Track))] int Id, string Name);
}