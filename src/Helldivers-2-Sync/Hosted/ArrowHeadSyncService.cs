using Helldivers.Core;
using Helldivers.Core.Storage;
using Helldivers.Models.ArrowHead;
using Helldivers.Sync.Configuration;
using Helldivers.Sync.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Helldivers.Sync.Hosted;

/// <summary>
/// The background synchronization service that pulls information from ArrowHead's API (through <see cref="ArrowHeadApiService" />)
/// and updates the <see cref="WarSnapshot" />.
/// </summary>
public sealed partial class ArrowHeadSyncService(
    ILogger<ArrowHeadSyncService> logger,
    IServiceScopeFactory scopeFactory,
    IOptions<HelldiversSyncConfiguration> configuration,
    ArrowHeadStore arrowHeadStore
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
        var languages = configuration.Value.Languages;
        var api = services.GetRequiredService<ArrowHeadApiService>();

        var season = await api.GetCurrentSeason(cancellationToken);
        var warInfo = await api.GetWarInfo(season, cancellationToken);
        var warSummary = await api.GetSummary(season, cancellationToken);

        // For each language, load war status.
        // If a language fails it's skipped.
        var statuses = await languages
            .ToAsyncEnumerable()
            .SelectAwait(language => AttemptToLoadWarStatus(api, season, language, cancellationToken))
            .Where(pair => pair.Value is not null)
            .ToDictionaryAsync(pair => pair.Key, pair => pair.Value!, cancellationToken);

        // For each language, load news feed.
        // If a language fails it's skipped.
        var feeds = await languages
            .ToAsyncEnumerable()
            .SelectAwait(language => AttemptToLoadTranslations(api.LoadFeed, season, language, cancellationToken))
            .Where(pair => pair.Value is not null)
            .ToDictionaryAsync(pair => pair.Key, pair => pair.Value!, cancellationToken);

        // For each language, load assignments
        // If a language fails it's skipped.
        var assignments = await languages
            .ToAsyncEnumerable()
            .SelectAwait(
                language => AttemptToLoadTranslations(api.LoadAssignments, season, language, cancellationToken))
            .Where(pair => pair.Value is not null)
            .ToDictionaryAsync(pair => pair.Key, pair => pair.Value!, cancellationToken);

        arrowHeadStore.UpdateSnapshot(warInfo, warSummary, statuses, feeds, assignments);
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
