using Helldivers.API;
using Helldivers.API.Configuration;
using Helldivers.API.Controllers;
using Helldivers.API.Controllers.V1;
using Helldivers.API.Metrics;
using Helldivers.API.Middlewares;
using Helldivers.Core.Extensions;
using Helldivers.Models;
using Helldivers.Models.Domain.Localization;
using Helldivers.Sync.Configuration;
using Helldivers.Sync.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Prometheus;
using System.Globalization;
using System.Net;
using System.Text.Json.Serialization;
using IPNetwork = Microsoft.AspNetCore.HttpOverrides.IPNetwork;

#if DEBUG
// When generating an OpenAPI document, get-document runs with the "--applicationName" flag.
// While detecting it this way isn't the 'prettiest' way, we *need* this information for following reasons:
// We don't want to start background services for sync etc when this flag is active
// And we *only* want to include OpenAPI generation stuff when this flag is active.
var isRunningAsTool = args.FirstOrDefault(arg => arg.StartsWith("--applicationName")) is not null;
#endif

var builder = WebApplication.CreateSlimBuilder(args);

#if RELEASE
// When running in release mode write JSON logfiles
builder.Logging.AddJsonConsole();
#endif

// Registers the core services in the container.
builder.Services.AddHelldivers();

// Have ASP.NET Core generate problemdetails for failed requests.
builder.Services.AddProblemDetails();

// Register the rate limiting middleware.
builder.Services.AddTransient<RateLimitMiddleware>();
builder.Services.AddTransient<RedirectFlyDomainMiddleware>();
builder.Services.AddTransient<BlacklistMiddleware>();

// Register the memory cache, used in the rate limiting middleware.
builder.Services.AddMemoryCache();

// Add services for response compression.
builder.Services.AddResponseCompression();

// Automatically set the CultureInfo based on the incoming request.
builder.Services.AddRequestLocalization(options =>
{
    var defaultLanguage = builder
        .Configuration
        .GetSection("Helldivers:Synchronization:DefaultLanguage")
        .Get<string>();

    var languages = builder
        .Configuration
        .GetSection("Helldivers:Synchronization:Languages")
        .Get<List<string>>()!;

    // Set the configured default language to be used by the LocalizedMessage class.
    LocalizedMessage.FallbackCulture = new CultureInfo(defaultLanguage ?? "en-US");

    options.ApplyCurrentCultureToResponseHeaders = true;
    options.DefaultRequestCulture = new RequestCulture(LocalizedMessage.FallbackCulture);
    options.SupportedCultures = languages.Select(iso => new CultureInfo(iso)).ToList();
    options.SupportedCultures.Add(LocalizedMessage.InvariantCulture);
    options.SupportedUICultures = options.SupportedCultures;
});
// Set CORS headers for websites directly accessing the API.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .WithHeaders(Constants.CLIENT_HEADER_NAME, Constants.CONTACT_HEADER_NAME)
    );
});

// Add and configure forwarded headers middleware
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardLimit = 999;
    options.OriginalForHeaderName = "Fly-Client-IP";
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor;
    options.KnownNetworks.Add(new IPNetwork(IPAddress.Any, 0));
    options.KnownNetworks.Add(new IPNetwork(IPAddress.IPv6Any, 0));
});

// This configuration is bound here so that source generators kick in.
builder.Services.Configure<ApiConfiguration>(builder.Configuration.GetSection("Helldivers:API"));
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

#if DEBUG
IdentityModelEventSource.ShowPII = true;
#endif

var authConfig = new AuthenticationConfiguration();
builder.Configuration.GetSection("Helldivers:API:Authentication").Bind(authConfig);
if (authConfig.Enabled)
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {

        options.TokenValidationParameters = new()
        {
            ValidIssuers = authConfig.ValidIssuers,
            ValidAudiences = authConfig.ValidAudiences,
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(authConfig.SigningKey)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });
    builder.Services.AddAuthorization();
}

// Swagger is generated at compile time, so we don't include Swagger dependencies in Release builds.
#if DEBUG
// Only add OpenApi dependencies when generating 
if (isRunningAsTool)
{
    builder.Services.AddOpenApiDocument(document =>
    {
        document.Title = "Helldivers 2";
        document.Description = "Helldivers 2 Unofficial API";

        var languages = builder
            .Configuration
            .GetSection("Helldivers:Synchronization:Languages")
            .Get<List<string>>()!;
        document.SchemaSettings.TypeMappers.Add(
            new Helldivers.API.OpenApi.TypeMappers.LocalizedMessageTypeMapper(languages)
        );

        document.OperationProcessors.Add(new Helldivers.API.OpenApi.OperationProcessors.SuperHeadersProcessor());

        document.DocumentProcessors.Add(new Helldivers.API.OpenApi.DocumentProcessors.HelldiversDocumentProcessor());
    });
    builder.Services.AddOpenApiDocument(document =>
    {
        document.Title = "ArrowHead API";
        document.Description = "An OpenAPI mapping of the official Helldivers API";
        document.DocumentName = "arrowhead";
        document.ApiGroupNames = ["arrowhead"];

        document.DocumentProcessors.Add(new Helldivers.API.OpenApi.DocumentProcessors.ArrowHeadDocumentProcessor());
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

app.UseMiddleware<RedirectFlyDomainMiddleware>();
app.UseMiddleware<BlacklistMiddleware>();

// Track telemetry for Prometheus (Fly.io metrics)
app.UseHttpMetrics(options =>
{
    options.AddCustomLabel("Client", ClientMetric.GetClientName);
});

// Use response compression for smaller payload sizes
app.UseResponseCompression();

// Enable static file host in case the application was built with OpenAPI specifications publicly available (Docker)
if (Directory.Exists(app.Environment.WebRootPath))
    app.UseStaticFiles();

// select the correct culture for incoming requests
app.UseRequestLocalization();

// Ensure web applications can access the API by setting CORS headers.
app.UseCors();

// Make sure ASP.NET Core uses the correct addresses internally rather than Fly's proxy
app.UseForwardedHeaders();

// Maps Prometheus to /metrics
app.MapMetrics();

// Handles rate limiting so everyone plays nice
app.UseMiddleware<RateLimitMiddleware>();

// Add middleware to timeout requests if they take too long.
app.UseRequestTimeouts();

#region API dev
#if DEBUG

var dev = app
    .MapGroup("/dev")
    .WithGroupName("development")
    .WithTags("dev")
    .ExcludeFromDescription();

dev.MapGet("/token", DevelopmentController.CreateToken);

#endif
#endregion

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
v1.MapGet("/planet-events", PlanetController.WithEvents);

v1.MapGet("/space-stations", SpaceStationController.Index);
v1.MapGet("/space/stations/{index:int}", SpaceStationController.Show);

v1.MapGet("/steam", SteamController.Index);
v1.MapGet("/steam/{gid}", SteamController.Show);

#endregion

app.MapGet("/", () => @"Check out the documentation over at https://helldivers-2.github.io/api/
You can also find additional resources on our Github: https://github.com/helldivers-2/api
Consider supporting us at https://github.com/sponsors/dealloc");

await app.RunAsync();
