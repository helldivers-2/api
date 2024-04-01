using Helldivers.API.Controllers;
using Helldivers.API.Controllers.V1;
using Helldivers.API.Middlewares;
using Helldivers.API.OpenApi;
using Helldivers.Core.Extensions;
using Helldivers.Models;
using Helldivers.Models.Domain.Localization;
using Helldivers.Sync.Configuration;
using Helldivers.Sync.Extensions;
using Microsoft.AspNetCore.Http.Timeouts;
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

// Registers the core services in the container.
builder.Services.AddHelldivers();

// Have ASP.NET Core generate problemdetails for failed requests.
builder.Services.AddProblemDetails();

// Register the rate limiting middleware.
builder.Services.AddTransient<RateLimitMiddleware>();

// Register the memory cache, used in the rate limiting middleware.
builder.Services.AddMemoryCache();

// Add services for response compression.
builder.Services.AddResponseCompression();

// Automatically set the CultureInfo based on the incoming request.
builder.Services.AddRequestLocalization(options =>
{
    var languages = builder
        .Configuration
        .GetSection("Helldivers:Synchronization:Languages")
        .Get<List<string>>()!;

    options.ApplyCurrentCultureToResponseHeaders = true;
    options.SupportedCultures = languages.Select(iso => new CultureInfo(iso)).ToList();
});
// Set CORS headers for websites directly accessing the API.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy
        .AllowAnyOrigin()
        .AllowAnyMethod()
    );
});

// Add and configure forwarded headers middleware
builder.Services.Configure<ForwardedHeadersOptions>(_ => { });

// This configuration is bound here so that source generators kick in.
builder.Services.Configure<HelldiversSyncConfiguration>(builder.Configuration.GetSection("Helldivers:Synchronization"));

// If a request takes over 10s to complete, abort it.
builder.Services.AddRequestTimeouts(options =>
{
    options.DefaultPolicy = new RequestTimeoutPolicy
    {
        Timeout = TimeSpan.FromSeconds(10),
        TimeoutStatusCode = StatusCodes.Status408RequestTimeout,
    };
});

// Setup source generated JSON type information so the API knows how to serialize models.
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Add(ArrowHeadSerializerContext.Default);
    options.SerializerOptions.TypeInfoResolverChain.Add(SteamSerializerContext.Default);
    options.SerializerOptions.TypeInfoResolverChain.Add(V1SerializerContext.Default);

    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

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
        
        document.DocumentProcessors.Add(new ArrowHeadDocumentProcessor());
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

var app = builder.Build();

// Use response compression for smaller payload sizes
app.UseResponseCompression();

// We host our OpenAPI spec and preview files from wwwroot/
app.UseStaticFiles();

// select the correct culture for incoming requests
app.UseRequestLocalization();

// Ensure web applications can access the API by setting CORS headers.
app.UseCors();

// Handles rate limiting so everyone plays nice
app.UseMiddleware<RateLimitMiddleware>();

// Make sure ASP.NET Core uses the correct addresses internally rather than Fly's proxy
app.UseForwardedHeaders();

// Add middleware to timeout requests if they take too long.
app.UseRequestTimeouts();

#region ArrowHead API endpoints ('raw' API)

var raw = app
    .MapGroup("/raw")
    .WithGroupName("arrowhead")
    .WithTags("raw");

raw.MapGet("/api/WarSeason/current/WarID", ArrowHeadController.WarId);
raw.MapGet("/api/WarSeason/801/Status", ArrowHeadController.Status);
raw.MapGet("/api/WarSeason/801/WarInfo", ArrowHeadController.WarInfo);
raw.MapGet("/api/Stats/war/801/summary", ArrowHeadController.Summary);
raw.MapGet("/api/NewsFeed/801", ArrowHeadController.NewsFeed);
raw.MapGet("/api/v2/Assignment/War/801", ArrowHeadController.Assignments);

#endregion

#region API v1

var v1 = app
    .MapGroup("/api/v1")
    .WithGroupName("community")
    .WithTags("v1");

v1.MapGet("/war", WarController.Show);

v1.MapGet("/assignments", AssignmentsController.Index);
v1.MapGet("/assignments/{index:long}", AssignmentsController.Show);

v1.MapGet("/campaigns", CampaignsController.Index);
v1.MapGet("/campaigns/{index:int}", CampaignsController.Show);

v1.MapGet("/dispatches", DispatchController.Index);
v1.MapGet("/dispatches/{index:int}", DispatchController.Show);

v1.MapGet("/planets", PlanetController.Index);
v1.MapGet("/planets/{index:int}", PlanetController.Show);

v1.MapGet("/steam", SteamController.Index);
v1.MapGet("/steam/{gid}", SteamController.Show);

#endregion

await app.RunAsync();
