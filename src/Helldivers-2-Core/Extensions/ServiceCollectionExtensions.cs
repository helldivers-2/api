using Helldivers.Core.Contracts;
using Helldivers.Core.Contracts.Collections;
using Helldivers.Core.Facades;
using Helldivers.Core.Mapping.Steam;
using Helldivers.Core.Mapping.V1;
using Helldivers.Core.Storage.ArrowHead;
using Helldivers.Core.Storage.Steam;
using Helldivers.Core.Storage.V1;
using Helldivers.Models.V1;
using Microsoft.Extensions.DependencyInjection;

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
        services.AddSingleton<StorageFacade>();

        return services
            .AddArrowHeadStores()
            .AddSteamStores()
            .AddV1Stores();
    }

    /// <summary>
    /// Registers all <see cref="IStore{T}" /> &amp; <see cref="IStore{T,TKey}" /> for ArrowHead models.
    /// </summary>
    public static IServiceCollection AddArrowHeadStores(this IServiceCollection services)
    {
        // Register all stores
        services.AddSingleton<ArrowHeadStore>();

        return services;
    }

    /// <summary>
    /// Registers all <see cref="IStore{T}" /> &amp; <see cref="IStore{T,TKey}" /> for Steam models.
    /// </summary>
    public static IServiceCollection AddSteamStores(this IServiceCollection services)
    {
        // Register facade for all stores below
        services.AddSingleton<SteamFacade>();

        // Register stores
        services.AddSingleton<IStore<SteamNews, string>, SteamNewsStore>();

        // Register mappers
        services.AddSingleton<SteamNewsMapper>();

        return services;
    }

    /// <summary>
    /// Registers all <see cref="IStore{T}" /> &amp; <see cref="IStore{T,TKey}" /> for V1 models.
    /// </summary>
    public static IServiceCollection AddV1Stores(this IServiceCollection services)
    {
        // Register facade for all stores below
        services.AddSingleton<V1Facade>();

        // Register stores
        services.AddSingleton<IStore<Planet, int>, PlanetStore>();
        services.AddSingleton<IStore<War>, WarStore>();
        services.AddSingleton<IStore<Campaign, int>, CampaignStore>();
        services.AddSingleton<IStore<Models.V1.Assignment, long>, Storage.V1.AssignmentStore>();
        services.AddSingleton<IStore<Dispatch, int>, DispatchStore>();
        services.AddSingleton<IStore<SpaceStation, int>, SpaceStationStore>();

        // Register mappers
        services.AddSingleton<AssignmentMapper>();
        services.AddSingleton<CampaignMapper>();
        services.AddSingleton<DispatchMapper>();
        services.AddSingleton<PlanetMapper>();
        services.AddSingleton<StatisticsMapper>();
        services.AddSingleton<WarMapper>();
        services.AddSingleton<SpaceStationMapper>();

        return services;
    }
}
