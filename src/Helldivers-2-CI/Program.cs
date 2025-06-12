using Helldivers.Core.Contracts.Collections;
using Helldivers.Core.Extensions;
using Helldivers.Models.Domain.Localization;
using Helldivers.Models.V1;
using Helldivers.Models.V2;
using Helldivers.Sync.Configuration;
using Helldivers.Sync.Extensions;
using Helldivers.Sync.Hosted;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using Dispatch = Helldivers.Models.V1.Dispatch;

var maxRuntime = new CancellationTokenSource(TimeSpan.FromSeconds(30));
var builder = new HostApplicationBuilder(args);

LocalizedMessage.FallbackCulture = new CultureInfo("en-US");
builder.Services.AddHelldivers();
builder.Services.AddHelldiversSync();
builder.Services.Configure<HelldiversSyncConfiguration>(configuration =>
{
    configuration.RunOnce = true;
    // Add all languages we want to CI test here.
    configuration.Languages =
    [
        "en-US",
        "de-DE",
        "es-ES",
        "ru-RU",
        "fr-FR",
        "it-IT",
        "pl-PL",
        "zh-Hans",
        "zh-Hant"
    ];
});

var stopwatch = new Stopwatch();
var app = builder.Build();

// Run our host, but make it shutdown after *max* 30 seconds.
stopwatch.Start();
var arrowhead = app.Services.GetRequiredService<ArrowHeadSyncService>();
var steam = app.Services.GetRequiredService<SteamSyncService>();
await arrowhead.StartAsync(maxRuntime.Token);
await steam.StartAsync(maxRuntime.Token);

// now we await completion of both.
await Task.WhenAll([
    arrowhead.ExecuteTask!,
    steam.ExecuteTask!,
]).WaitAsync(maxRuntime.Token);
stopwatch.Stop();

app.Services.GetRequiredService<ILogger<Program>>().LogInformation("Sync succeeded in {}", stopwatch.Elapsed);

// Fetch all information from the stores to write to disk.
var assignments = await app.Services.GetRequiredService<IStore<Assignment, long>>().AllAsync(maxRuntime.Token);
var campaigns = await app.Services.GetRequiredService<IStore<Campaign, int>>().AllAsync(maxRuntime.Token);
var dispatchesv1 = await app.Services.GetRequiredService<IStore<Dispatch, int>>().AllAsync(maxRuntime.Token);
var planets = await app.Services.GetRequiredService<IStore<Planet, int>>().AllAsync(maxRuntime.Token);
var war = await app.Services.GetRequiredService<Helldivers.Core.Contracts.IStore<War>>().Get(maxRuntime.Token);
var dispatchesv2 = await app.Services.GetRequiredService<IStore<Helldivers.Models.V2.Dispatch, int>>().AllAsync(maxRuntime.Token);
var store = await app.Services.GetRequiredService<IStore<SpaceStation, long>>().AllAsync(maxRuntime.Token);

Directory.CreateDirectory("v1");
Directory.CreateDirectory("v2");

var options = new JsonSerializerOptions { WriteIndented = true };
await File.WriteAllBytesAsync("v1/assignments.json", JsonSerializer.SerializeToUtf8Bytes(assignments, options), maxRuntime.Token);
await File.WriteAllBytesAsync("v1/campaigns.json", JsonSerializer.SerializeToUtf8Bytes(campaigns, options), maxRuntime.Token);
await File.WriteAllBytesAsync("v1/dispatches.json", JsonSerializer.SerializeToUtf8Bytes(dispatchesv1, options), maxRuntime.Token);
await File.WriteAllBytesAsync("v1/planets.json", JsonSerializer.SerializeToUtf8Bytes(planets, options), maxRuntime.Token);
await File.WriteAllBytesAsync("v1/war.json", JsonSerializer.SerializeToUtf8Bytes(war, options), maxRuntime.Token);
await File.WriteAllBytesAsync("v2/dispatches.json", JsonSerializer.SerializeToUtf8Bytes(dispatchesv2, options), maxRuntime.Token);
await File.WriteAllBytesAsync("v2/space-stations.json", JsonSerializer.SerializeToUtf8Bytes(store, options), maxRuntime.Token);

return 0;
