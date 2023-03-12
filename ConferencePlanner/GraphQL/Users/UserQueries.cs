using HotChocolate.AspNetCore;
using HotChocolate.Execution;

namespace ConferencePlanner.GraphQL.Users
{
    [QueryType]
    public class UserQueries
    {
        public string GetUser([GlobalState] int? currentUserId = null)
        {
            if (currentUserId is null)
            {
                return "Nick";
            }

            return "Mac";
        }
    }

    public class CustomHttpRequestInterceptor : DefaultHttpRequestInterceptor
    {
        public override ValueTask OnCreateAsync(HttpContext context, IRequestExecutor requestExecutor, IQueryRequestBuilder requestBuilder,
            CancellationToken cancellationToken)
        {
            requestBuilder.TryAddGlobalState("currentUserId", 69);
            return base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
        }
    }
}