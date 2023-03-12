using ConferencePlanner.Data;
using ConferencePlanner.GraphQL;
using ConferencePlanner.GraphQL.Users;
using ConferencePlanner.Logging;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();
builder.Services.AddGraphQLServer()
    .RegisterDbContext<ApplicationDbContext>()
    .AddConferencePlannerTypes()
    .AddDataLoader<SpeakerByIdDataLoader>()
    .AddGlobalObjectIdentification()
    .AddFiltering()
    .AddSorting()
    .AddHttpRequestInterceptor<CustomHttpRequestInterceptor>()
    //.AddDiagnosticEventListener<ConsoleQueryLogger>()
    .AddDiagnosticEventListener(sp => new MiniProfilerQueryLogger())
    .AddInMemorySubscriptions();

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