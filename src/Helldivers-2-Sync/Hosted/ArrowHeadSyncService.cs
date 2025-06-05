using Helldivers.Core;
using Helldivers.Models.ArrowHead;
using Helldivers.Sync.Configuration;
using Helldivers.Sync.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prometheus;
using System.Globalization;

namespace Helldivers.Sync.Hosted;

/// <summary>
/// The background synchronization service that pulls information from ArrowHead's API (through <see cref="ArrowHeadApiService" />)
/// and updates the <see cref="StorageFacade" />.
/// </summary>
public sealed partial class ArrowHeadSyncService(
    ILogger<ArrowHeadSyncService> logger,
    IServiceScopeFactory scopeFactory,
    IOptions<HelldiversSyncConfiguration> configuration,
    StorageFacade storage
) : BackgroundService
{
    /// <summary>
    /// Timestamp the store was last updated successfully.
    /// </summary>
    public DateTime? LastUpdated { get; private set; }

    private static readonly Histogram ArrowHeadSyncMetric =
        Metrics.CreateHistogram("helldivers_sync_arrowhead", "All ArrowHead synchronizations");

    #region Source generated logging

    [LoggerMessage(Level = LogLevel.Information, Message = "sync will run every {Interval}")]
    private static partial void LogRunAtInterval(ILogger logger, TimeSpan interval);

    [LoggerMessage(LogLevel.Debug, Message = "Running sync at {Timestamp}")]
    private static partial void LogRunningSyncAt(ILogger logger, DateTime timestamp);

    [LoggerMessage(Level = LogLevel.Error, Message = "An exception was thrown when synchronizing from ArrowHead API")]
    private static partial void LogSyncThrewAnError(ILogger logger, Exception exception);

    [LoggerMessage(LogLevel.Warning, Message = "Failed to download translations for {Language} of {Type}")]
    private static partial void LogFailedToLoadTranslation(ILogger logger, Exception exception, string language,
        string type);

    [LoggerMessage(Level = LogLevel.Information, Message = "ArrowHeadSyncService finished processing, shutting down.")]
    private static partial void LogRunOnceCompleted(ILogger logger);

    #endregion

    /// <inheritdoc cref="BackgroundService.ExecuteAsync(CancellationToken)" />
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {

        var delay = TimeSpan.FromSeconds(configuration.Value.IntervalSeconds);

        LogRunAtInterval(logger, delay);
        while (cancellationToken.IsCancellationRequested is false)
        {
            try
            {
                using var _ = ArrowHeadSyncMetric.NewTimer();
                await using var scope = scopeFactory.CreateAsyncScope();

                LogRunningSyncAt(logger, DateTime.UtcNow);
                await SynchronizeAsync(scope.ServiceProvider, cancellationToken);
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

    private async Task SynchronizeAsync(IServiceProvider services, CancellationToken cancellationToken)
    {
        var api = services.GetRequiredService<ArrowHeadApiService>();

        var (rawWarId, warId) = await api.GetCurrentSeason(cancellationToken);
        var season = warId.Id.ToString(CultureInfo.InvariantCulture);
        var warInfo = await api.GetWarInfo(season, cancellationToken);
        var warSummary = await api.GetSummary(season, cancellationToken);

        // For each language, load war status.
        var statuses = await DownloadTranslations<WarStatus>(
            language => api.GetWarStatus(season, language, cancellationToken),
            cancellationToken
        );

        // For each language, load news feed.
        var feeds = await DownloadTranslations<NewsFeedItem>(
            async language => await api.LoadFeed(season, language, cancellationToken),
            cancellationToken
        );

        // For each language, load assignments
        var assignments = await DownloadTranslations<Assignment>(
            async language => await api.LoadAssignments(season, language, cancellationToken),
            cancellationToken
        );

        var spaceStations = await configuration.Value.SpaceStations.ToAsyncEnumerable().ToDictionaryAwaitAsync(ValueTask.FromResult, async key =>
            await DownloadTranslations<SpaceStation>(
                async language => await api.LoadSpaceStations(season, key, language, cancellationToken),
                cancellationToken
            ), cancellationToken: cancellationToken);

        await storage.UpdateStores(
            rawWarId,
            warInfo,
            statuses,
            warSummary,
            feeds,
            assignments,
            spaceStations
        );

        LastUpdated = DateTime.UtcNow;
    }

    private async Task<Dictionary<string, Memory<byte>>> DownloadTranslations<T>(Func<string, Task<Memory<byte>>> func,
        CancellationToken cancellationToken)
    {
        return await configuration.Value.Languages
            .ToAsyncEnumerable()
            .Select(async language =>
            {
                try
                {
                    var result = await func(language);

                    return new KeyValuePair<string, Memory<byte>?>(language, result);
                }
                catch (Exception exception)
                {
                    LogFailedToLoadTranslation(logger, exception, language, typeof(T).Name);

                    return new KeyValuePair<string, Memory<byte>?>(language, null);
                }
            })
            .SelectAwait(async task => await task)
            .Where(pair => pair.Value is not null)
            .ToDictionaryAsync(pair => pair.Key, pair => pair.Value.GetValueOrDefault(),
                cancellationToken: cancellationToken);
    }
}
