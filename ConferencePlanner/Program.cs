using ConferencePlanner.Data;
using ConferencePlanner.GraphQL;
using ConferencePlanner.GraphQL.Sessions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddGraphQLServer()
    .RegisterDbContext<ApplicationDbContext>()
    .AddConferencePlannerTypes()
    .AddDataLoader<SpeakerByIdDataLoader>()
    .AddGlobalObjectIdentification()
    .AddFiltering()
    .AddSorting()
    .AddInMemorySubscriptions();
//.AddProjections();


var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.UseWebSockets();
app.MapGraphQL();

app.Run();