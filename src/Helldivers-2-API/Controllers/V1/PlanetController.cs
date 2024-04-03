using Helldivers.Core.Contracts.Collections;
using Helldivers.Models.V1;
using Microsoft.AspNetCore.Mvc;

namespace Helldivers.API.Controllers.V1;

/// <summary>
/// Contains API endpoints for <see cref="Planet" />.
/// </summary>
public static class PlanetController
{
    /// <summary>
    /// Fetches a list of all available <see cref="Planet" /> information available.
    /// </summary>
    [ProducesResponseType<List<Planet>>(StatusCodes.Status200OK)]
    public static async Task<IResult> Index(HttpContext context, IStore<Planet, int> store)
    {
        var planets = await store.AllAsync(context.RequestAborted);

        return Results.Ok(planets);
    }


    /// <summary>
    /// Fetches a specific <see cref="Planet" /> identified by <paramref name="index" />.
    /// </summary>
    [ProducesResponseType<Planet>(StatusCodes.Status200OK)]
    public static async Task<IResult> Show(HttpContext context, IStore<Planet, int> store, [FromRoute] int index)
    {
        var planet = await store.GetAsync(index, context.RequestAborted);
        if (planet is null)
            return Results.NotFound();

        return Results.Ok(planet);
    }

    /// <summary>
    /// Fetches all planets with an active <see cref="Planet.Event" />.
    /// </summary>
    [ProducesResponseType<List<Planet>>(StatusCodes.Status200OK)]
    public static async Task<IResult> WithEvents(HttpContext context, IStore<Planet, int> store)
    {
        var planets = await store.AllAsync(context.RequestAborted);
        var withEvents = planets
            .Where(planet => planet.Event is not null)
            .ToList();

        return Results.Ok(withEvents);
    }
}
