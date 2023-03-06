using ConferencePlanner.Data.Entities;
using ConferencePlanner.GraphQL.Common;

namespace ConferencePlanner.GraphQL.Sessions;

public class AddSessionPayload : SessionPayloadBase
{
    public AddSessionPayload(UserError error) 
        : base(new[] { error })
    {
    }

    public AddSessionPayload(Session session) 
        : base(session)
    {
    }

    public AddSessionPayload(IReadOnlyList<UserError> errors) 
        : base(errors)
    {
    }
}