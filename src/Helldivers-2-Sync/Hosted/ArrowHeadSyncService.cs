using Helldivers.Sync.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Helldivers.Sync.Hosted;

public sealed partial class ArrowHeadSyncService(
    ILogger<ArrowHeadSyncService> logger,
    IOptions<HelldiversSyncConfiguration> configuration
) : BackgroundService
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Running sync service")]
    private static partial void LogExecuting(ILogger logger);

    /// <inheritdoc cref="BackgroundService.ExecuteAsync(CancellationToken)" />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var delay = TimeSpan.FromSeconds(configuration.Value.IntervalSeconds);

        while (stoppingToken.IsCancellationRequested is false)
        {
            LogExecuting(logger);

            await Task.Delay(delay, stoppingToken);
        }
    }
}
