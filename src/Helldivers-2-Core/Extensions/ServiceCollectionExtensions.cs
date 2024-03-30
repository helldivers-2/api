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
        // Add the singleton snapshot state.
        services.AddSingleton<WarSnapshot>();
        services.AddSingleton<SteamSnapshot>();

        return services;
    }
}
