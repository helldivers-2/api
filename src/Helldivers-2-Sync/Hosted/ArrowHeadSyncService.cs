using Helldivers.Core;
using Helldivers.Models.ArrowHead;
using Helldivers.Sync.Configuration;
using Helldivers.Sync.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
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
    #region Source generated logging

    [LoggerMessage(Level = LogLevel.Information, Message = "sync will run every {Interval}")]
    private static partial void LogRunAtInterval(ILogger logger, TimeSpan interval);

    [LoggerMessage(Level = LogLevel.Error, Message = "An exception was thrown when synchronizing from ArrowHead API")]
    private static partial void LogSyncThrewAnError(ILogger logger, Exception exception);

    [LoggerMessage(LogLevel.Information, Message = "Finished synchronizing from ArrowHead API in {Duration}")]
    private static partial void LogFinishedSynchronize(ILogger logger, TimeSpan duration);

    [LoggerMessage(LogLevel.Warning, Message = "Failed to download translations for {Language} of {Type}")]
    private static partial void LogFailedToLoadTranslation(ILogger logger, Exception exception, string language,
        string type);

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
                var stopwatch = new Stopwatch();
                await using var scope = scopeFactory.CreateAsyncScope();

                stopwatch.Start();
                await SynchronizeAsync(scope.ServiceProvider, cancellationToken);
                stopwatch.Stop();

                LogFinishedSynchronize(logger, stopwatch.Elapsed);
            }
            catch (Exception exception)
            {
                LogSyncThrewAnError(logger, exception);
            }

            await Task.Delay(delay, cancellationToken);
        }
    }

    private async Task SynchronizeAsync(IServiceProvider services, CancellationToken cancellationToken)
    {
        var api = services.GetRequiredService<ArrowHeadApiService>();

        var warId = await api.GetCurrentSeason(cancellationToken);
        var season = warId.Id.ToString(CultureInfo.InvariantCulture);
        var warInfo = await api.GetWarInfo(season, cancellationToken);
        var warSummary = await api.GetSummary(season, cancellationToken);

        // For each language, load war status.
        var statuses = await DownloadTranslations(
            language => api.GetWarStatus(season, language, cancellationToken),
            cancellationToken
        );

        // For each language, load news feed.
        var feeds = await DownloadTranslations(
            async language => await api.LoadFeed(season, language, cancellationToken).ToListAsync(cancellationToken),
            cancellationToken
        );

        // For each language, load assignments
        var assignments = await DownloadTranslations(
            async language => await api.LoadAssignments(season, language, cancellationToken).ToListAsync(cancellationToken),
            cancellationToken
        );

        await storage.UpdateStores(
            warId,
            warInfo,
            statuses,
            warSummary,
            feeds,
            assignments
        );
    }

    private async Task<Dictionary<string, T>> DownloadTranslations<T>(Func<string, Task<T>> func, CancellationToken cancellationToken) where T : class
    {
        return await configuration.Value.Languages
            .ToAsyncEnumerable()
            .Select(async language =>
            {
                try
                {
                    var result = await func(language);

                    return new KeyValuePair<string, T?>(language, result);
                }
                catch (Exception exception)
                {
                    LogFailedToLoadTranslation(logger, exception, language, typeof(T).Name);

                    return new KeyValuePair<string, T?>(language, null);
                }
            })
            .SelectAwait(async task => await task)
            .Where(pair => pair.Value is not null)
            .ToDictionaryAsync(pair => pair.Key, pair => pair.Value!, cancellationToken: cancellationToken);
    }

    /// <summary>Helper function to download the war status or return null if anything fails.</summary>
    private async ValueTask<KeyValuePair<string, WarStatus?>> AttemptToLoadWarStatus(ArrowHeadApiService arrowHeadApi, string season,
        string language, CancellationToken cancellationToken)
    {
        try
        {
            var status = await arrowHeadApi.GetWarStatus(season, language, cancellationToken);

            return new(language, status);
        }
        catch (Exception exception)
        {
            LogFailedToLoadTranslation(logger, exception, language, nameof(WarStatus));

            return new(language, null);
        }
    }

    /// <summary>Helper function to download a list of <typeparamref name="T" /> or return null if anything fails.</summary>
    private async ValueTask<KeyValuePair<string, List<T>?>> AttemptToLoadTranslations<T>(
        Func<string, string, CancellationToken, IAsyncEnumerable<T>> api, string season, string language,
        CancellationToken cancellationToken)
    {
        try
        {
            var items = await api(season, language, cancellationToken).ToListAsync(cancellationToken);

            return new(language, items);
        }
        catch (Exception exception)
        {
            LogFailedToLoadTranslation(logger, exception, language, typeof(T).Name);

            return new(language, null);
        }
    }
}
