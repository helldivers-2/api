using Helldivers.Sync.Hosted;
using Helldivers.Sync.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Helldivers.Sync.Extensions;

/// <summary>
/// Contains extension methods for <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds all services related to the Helldivers API to the service container.
    /// </summary>
    public static IServiceCollection AddHelldiversSync(this IServiceCollection services)
    {
        services.AddHostedService<ArrowHeadSyncService>();
        services.AddHostedService<SteamSyncService>();
        services.AddHttpClient<SteamApiService>();
        services.AddHttpClient<ArrowHeadApiService>(http =>
        {
            http.BaseAddress = new Uri("https://api.live.prod.thehelldiversgame.com");
        });

        return services;
    }
}
