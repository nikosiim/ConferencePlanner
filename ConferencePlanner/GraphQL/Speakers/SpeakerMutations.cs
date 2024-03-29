﻿using ConferencePlanner.Data;
using ConferencePlanner.Data.Entities;

namespace ConferencePlanner.GraphQL.Speakers
{
    [MutationType]
    public class SpeakerMutations
    {
        public async Task<AddSpeakerPayload> AddSpeakerAsync(AddSpeakerInput input, ApplicationDbContext context)
        {
            var speaker = new Speaker
            {
                Name = input.Name,
                Bio = input.Bio,
                WebSite = input.WebSite
            };

            context.Speakers.Add(speaker);
            await context.SaveChangesAsync();

            return new AddSpeakerPayload(speaker);
        }
    }
}