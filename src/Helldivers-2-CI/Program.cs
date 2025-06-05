using Helldivers.Core.Extensions;
using Helldivers.Sync.Configuration;
using Helldivers.Sync.Extensions;
using Helldivers.Sync.Hosted;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

var maxRuntime = new CancellationTokenSource(TimeSpan.FromSeconds(30));
var builder = new HostApplicationBuilder(args);

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
var steam =  app.Services.GetRequiredService<SteamSyncService>();
await arrowhead.StartAsync(maxRuntime.Token);
await steam.StartAsync(maxRuntime.Token);

// now we await completion of both.
await Task.WhenAll([
    arrowhead.ExecuteTask!,
    steam.ExecuteTask!,
]).WaitAsync(maxRuntime.Token);
stopwatch.Stop();

app.Services.GetRequiredService<ILogger<Program>>().LogInformation("Sync succeeded in {}", stopwatch.Elapsed);

return 0;
