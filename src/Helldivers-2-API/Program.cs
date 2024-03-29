using Helldivers.API.Controllers;
using Helldivers.API.Controllers.V1;
using Helldivers.Core.Extensions;
using Helldivers.Models;
using Helldivers.Models.Domain.Localization;
using Helldivers.Sync.Configuration;
using Helldivers.Sync.Extensions;
using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;
using System.Globalization;
using System.Text.Json.Serialization;

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
    builder.Services.AddOpenApiDocument(document =>
    {
        document.Title = "Helldivers 2";
        document.Description = "Helldivers 2 Unofficial API";
        
        document.SchemaSettings.TypeMappers.Add(
            new PrimitiveTypeMapper(
                typeof(LocalizedMessage),
                schema => schema.Type = JsonObjectType.String
            )
        );
    });
    builder.Services.AddOpenApiDocument(document =>
    {
        document.Title = "ArrowHead API";
        document.Description = "An OpenAPI mapping of the official Helldivers API";
        document.DocumentName = "arrowhead";
        document.ApiGroupNames = ["arrowhead"];
    });
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
{
    options.SerializerOptions.TypeInfoResolverChain.Add(ArrowHeadSerializerContext.Default);
    options.SerializerOptions.TypeInfoResolverChain.Add(HelldiversJsonSerializerContext.Default);
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// We host our OpenAPI spec and preview files from wwwroot/
app.UseStaticFiles();

// select the correct culture for incoming requests
app.UseRequestLocalization();

// Ensure web applications can access the API by setting CORS headers.
app.UseCors();

app.MapGet("/health", HealthController.Show).ExcludeFromDescription();

#region ArrowHead API endpoints ('raw' API)

var raw = app
    .MapGroup("/")
    .WithGroupName("arrowhead")
    .WithTags("raw");

raw.MapGet("/api/WarSeason/current/WarID", ArrowHeadController.WarId);
raw.MapGet("/api/WarSeason/801/Status", ArrowHeadController.Status);
raw.MapGet("/api/WarSeason/801/WarInfo", ArrowHeadController.WarInfo);
raw.MapGet("/api/Stats/war/801/summary", ArrowHeadController.Summary);
raw.MapGet("/api/NewsFeed/801", ArrowHeadController.NewsFeed);
raw.MapGet("/api/v2/Assignment/War/801", ArrowHeadController.Assignment);

#endregion

#region API v1

var v1 = app
    .MapGroup("/api/v1")
    .WithGroupName("community")
    .WithTags("v1");

v1.MapGet("/", GalaxyWarController.Show);
v1.MapGet("/war-id", GalaxyWarController.ShowWarId);

v1.MapGet("/planets", PlanetsController.Index);
v1.MapGet("/planets/{index:int}", PlanetsController.Show);
v1.MapGet("/planets/{index:int}/statistics", PlanetsController.ShowStatistics);

v1.MapGet("/news", NewsFeedController.Index);
v1.MapGet("/news/{index:int}", NewsFeedController.Show);

v1.MapGet("/assignments", AssignmentsController.Index);
v1.MapGet("/assignments/{index:long}", AssignmentsController.Show);

v1.MapGet("/test", () => LocalizedMessage.FromStrings(new Dictionary<string, string>
{
    { "en-US", "English" },
    { "de-DE", "Deutsch" },
})).Produces<string>();

#endregion

await app.RunAsync();
