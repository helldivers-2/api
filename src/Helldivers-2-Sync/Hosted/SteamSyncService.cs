using Helldivers.Core;
using Helldivers.Sync.Configuration;
using Helldivers.Sync.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prometheus;

namespace Helldivers.Sync.Hosted;

/// <summary>
/// Handles synchronization of Steam API information.
/// </summary>
public sealed partial class SteamSyncService(
    ILogger<SteamSyncService> logger,
    StorageFacade storage,
    IServiceScopeFactory scopeFactory,
    IOptions<HelldiversSyncConfiguration> configuration
) : BackgroundService
{
    /// <summary>
    /// Timestamp the store was last updated successfully.
    /// </summary>
    public DateTime? LastUpdated { get; internal set; }

    private static readonly Histogram SteamSyncMetric =
        Metrics.CreateHistogram("helldivers_sync_steam", "All Steam synchronizations");

    #region Source generated logging

    [LoggerMessage(Level = LogLevel.Error, Message = "An exception was thrown when synchronizing from Steam API")]
    private static partial void LogSyncThrewAnError(ILogger logger, Exception exception);

    [LoggerMessage(Level = LogLevel.Information, Message = "SteamSyncService finished processing, shutting down.")]
    private static partial void LogRunOnceCompleted(ILogger logger);

    #endregion

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var delay = TimeSpan.FromHours(1);

        while (cancellationToken.IsCancellationRequested is false)
        {
            try
            {
                using var _ = SteamSyncMetric.NewTimer();
                await using var scope = scopeFactory.CreateAsyncScope();
                var api = scope.ServiceProvider.GetRequiredService<SteamApiService>();

                var feed = await api.GetLatest();

                await storage.UpdateStores(feed);
                LastUpdated = DateTime.UtcNow;
            }
            catch (Exception exception)
            {
                LogSyncThrewAnError(logger, exception);

                if (configuration.Value.RunOnce)
                    throw;
            }


            // If we should only run once, we exit the loop (and thus service) after this.
            if (configuration.Value.RunOnce)
            {
                LogRunOnceCompleted(logger);
                return;
            }
            await Task.Delay(delay, cancellationToken);
        }
    }
}
