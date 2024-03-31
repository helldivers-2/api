using Helldivers.Core.Contracts;
using Helldivers.Core.Contracts.Collections;
using Helldivers.Core.Storage;
using Helldivers.Models.ArrowHead;
using Helldivers.Models.Domain;
using Microsoft.Extensions.DependencyInjection;
using Assignment = Helldivers.Models.ArrowHead.Assignment;

namespace Helldivers.Core.Extensions;

/// <summary>
/// Contains extension methods for <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds all services related to the Helldivers API to the service container.
    /// </summary>
    public static IServiceCollection AddHelldivers(this IServiceCollection services)
    {
        // Add the singleton snapshot state.
        services.AddSingleton<WarSnapshot>();
        services.AddSingleton<SteamSnapshot>();

        // Register ArrowHead stores
        services.AddSingleton<ArrowHeadStore>();
        services.AddSingleton<IStore<WarInfo>>(provider => provider.GetRequiredService<ArrowHeadStore>());
        services.AddSingleton<IStore<WarStatus>>(provider => provider.GetRequiredService<ArrowHeadStore>());
        services.AddSingleton<IStore<WarSummary>>(provider => provider.GetRequiredService<ArrowHeadStore>());
        services.AddSingleton<IStore<Assignment, int>>(provider => provider.GetRequiredService<ArrowHeadStore>());
        services.AddSingleton<IStore<NewsFeedItem, int>>(provider => provider.GetRequiredService<ArrowHeadStore>());

        // Register Steam store
        services.AddSingleton<SteamStore>();
        services.AddSingleton<IStore<SteamNews, string>>(provider => provider.GetRequiredService<SteamStore>());

        return services;
    }
}
