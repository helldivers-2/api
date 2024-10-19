using Helldivers.Core;
using Helldivers.Sync.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prometheus;

namespace Helldivers.Sync.Hosted;

/// <summary>
/// Handles synchronization of Steam API information.
/// </summary>
public sealed partial class SteamSyncService(
    ILogger<SteamSyncService> logger,
    StorageFacade storage,
    IServiceScopeFactory scopeFactory
) : BackgroundService
{
    private static readonly Histogram SteamSyncMetric =
        Metrics.CreateHistogram("helldivers_sync_steam", "All Steam synchronizations");

    #region Source generated logging

    [LoggerMessage(Level = LogLevel.Error, Message = "An exception was thrown when synchronizing from Steam API")]
    private static partial void LogSyncThrewAnError(ILogger logger, Exception exception);

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

                var feed = await scope
                    .ServiceProvider
                    .GetRequiredService<SteamApiService>()
                    .GetLatest();

                await storage.UpdateStores(feed);
            }
            catch (Exception exception)
            {
                LogSyncThrewAnError(logger, exception);
            }

            await Task.Delay(delay, cancellationToken);
        }
    }
}
