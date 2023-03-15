using ConferencePlanner.Data;
using ConferencePlanner.GraphQL.Users;
using ConferencePlanner.Logging;
using HotChocolate.Types.Pagination;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();
builder.Services.AddGraphQLServer()
    .RegisterDbContext<ApplicationDbContext>()
    .AddConferencePlannerTypes()
    .AddGlobalObjectIdentification()
    .AddFiltering()
    .AddSorting()
    .AddHttpRequestInterceptor<CustomHttpRequestInterceptor>()
    //.AddDiagnosticEventListener<ConsoleQueryLogger>()
    .AddDiagnosticEventListener(sp => new MiniProfilerQueryLogger())
    .AddInMemorySubscriptions()
    .SetPagingOptions(new PagingOptions{IncludeTotalCount = true})
    .ModifyRequestOptions(o => o.IncludeExceptionDetails = true); // To see the exception in the response beside the unexpected execution error

    /*
        // If you want to declare fields explicitly on all types in your schema, you can set the following option
        .ModifyOptions(o => o.DefaultBindingBehavior = BindingBehavior.Explicit)
    */

    /*
        // To remove [GlobalState] attribute from resolver
        builder.Services.AddSingleton<IParameterExpressionBuilder>(new CustomParameterExpressionBuilder<int?>(
            c => c.GetGlobalState<int?>("currentUserId"), p => p.Name.EqualsOrdinal("currentUserId")));
    */

// The results can be seen at: http://localhost:{port number}/profiler/results-index
builder.Services
    .AddMiniProfiler(options => { options.RouteBasePath = "/profiler"; }) 
    .AddEntityFramework();

var app = builder.Build();

app.UseWebSockets();
app.UseMiniProfiler();
app.MapGraphQL();

app.MapGet("/", () => "Hello World!");
app.Run();