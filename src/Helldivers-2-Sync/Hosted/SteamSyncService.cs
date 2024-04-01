using Helldivers.Core;
using Helldivers.Sync.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Helldivers.Sync.Hosted;

public sealed partial class SteamSyncService(
    ILogger<SteamSyncService> logger,
    StorageFacade storage,
    IServiceScopeFactory scopeFactory
) : BackgroundService
{
    #region Source generated logging

    [LoggerMessage(Level = LogLevel.Error, Message = "An exception was thrown when synchronizing from Steam API")]
    private static partial void LogSyncThrewAnError(ILogger logger, Exception exception);

    [LoggerMessage(LogLevel.Information, Message = "Finished synchronizing from Steam API in {Duration}")]
    private static partial void LogFinishedSynchronize(ILogger logger, TimeSpan duration);

    #endregion

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var delay = TimeSpan.FromHours(1);

        while (cancellationToken.IsCancellationRequested is false)
        {
            try
            {
                var stopwatch = new Stopwatch();
                await using var scope = scopeFactory.CreateAsyncScope();

                stopwatch.Start();

                var feed = await scope
                    .ServiceProvider
                    .GetRequiredService<SteamApiService>()
                    .GetLatest();

                await storage.UpdateStores(feed);

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
}
