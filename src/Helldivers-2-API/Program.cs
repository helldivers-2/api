using Helldivers.API.Controllers;
using Helldivers.Core.Extensions;
using Helldivers.Models;
using Helldivers.Sync.Configuration;
using Helldivers.Sync.Extensions;
using System.Globalization;

#if DEBUG
// When generating an OpenAPI document, get-document runs with the "--applicationName" flag.
// While detecting it this way isn't the 'prettiest' way, we *need* this information for following reasons:
// We don't want to start background services for sync etc when this flag is active
// And we *only* want to include OpenAPI generation stuff when this flag is active.
var isRunningAsTool = args.FirstOrDefault(arg => arg.StartsWith("--applicationName")) is not null;
#endif

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddHelldivers();
builder.Services.AddProblemDetails();
builder.Services.AddRequestLocalization(options =>
{
    var languages = builder
        .Configuration
        .GetSection("Helldivers:Synchronization:Languages")
        .Get<List<string>>()!;

    options.ApplyCurrentCultureToResponseHeaders = true;
    options.SupportedCultures = languages.Select(iso => new CultureInfo(iso)).ToList();
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy
        .AllowAnyOrigin()
        .AllowAnyMethod()
    );
});

// This configuration is bound here so that source generators kick in.
builder.Services.Configure<HelldiversSyncConfiguration>(builder.Configuration.GetSection("Helldivers:Synchronization"));

// Swagger is generated at compile time, so we don't include Swagger dependencies in Release builds.
#if DEBUG
// Only add OpenApi dependencies when generating 
if (isRunningAsTool)
{
    builder.Services.AddOpenApiDocument();
    builder.Services.AddEndpointsApiExplorer();
}
else
{
    builder.Services.AddHelldiversSync();
}
#else
// in Release builds we *always* run the sync services
builder.Services.AddHelldiversSync();
#endif

// Setup source generated JSON type information so the API knows how to serialize models.
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.TypeInfoResolverChain.Add(HelldiversJsonSerializerContext.Default)
);

var app = builder.Build();

// We host our OpenAPI spec and preview files from wwwroot/
app.UseStaticFiles();

// select the correct culture for incoming requests
app.UseRequestLocalization();

// Ensure web applications can access the API by setting CORS headers.
app.UseCors();

#region ArrowHead API endpoints ('raw' API)

var raw = app
    .MapGroup("/")
    .WithTags("arrowhead");

raw.MapGet("/api/WarSeason/current/WarID", ArrowHeadController.WarId);
raw.MapGet("/api/WarSeason/801/Status", ArrowHeadController.Status);
raw.MapGet("/api/WarSeason/801/WarInfo", ArrowHeadController.WarInfo);
raw.MapGet("/api/NewsFeed/801", ArrowHeadController.NewsFeed);
raw.MapGet("/api/v2/Assignment/War/801", ArrowHeadController.Assignment);

#endregion


await app.RunAsync();
