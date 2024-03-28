using Helldivers.API.Controllers;
using Helldivers.Models;
using Helldivers.Sync.Configuration;
using Helldivers.Sync.Hosted;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddHostedService<ArrowHeadSyncService>();
builder.Services.Configure<HelldiversSyncConfiguration>(builder.Configuration.GetSection("Helldivers:Synchronization"));

#if DEBUG
// Swagger is generated at compile time, so we don't include Swagger dependencies in Release builds.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument();
#endif

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Add(HelldiversJsonSerializerContext.Default);
});

var app = builder.Build();

app.UseStaticFiles();

app.MapGet("/war-season", WarSeasonController.Current);

await app.RunAsync();
