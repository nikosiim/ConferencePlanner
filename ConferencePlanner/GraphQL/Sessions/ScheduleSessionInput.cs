﻿using ConferencePlanner.Data.Entities;

namespace ConferencePlanner.GraphQL.Sessions
{
    // Note that when adding the annotation to the constructor input,
    // you must add the "property" specification, otherwise it will not apply to the property.
    public record ScheduleSessionInput(
        [property: ID<Session>] int SessionId, 
        [property: ID<Track>] int TrackId, 
        DateTimeOffset StartTime, 
        DateTimeOffset EndTime);
}