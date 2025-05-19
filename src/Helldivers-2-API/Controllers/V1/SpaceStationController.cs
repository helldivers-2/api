using Helldivers.Core.Contracts.Collections;
using Helldivers.Models.V1;
using Microsoft.AspNetCore.Mvc;

namespace Helldivers.API.Controllers.V1;

/// <summary>
/// Contains API endpoints for <see cref="SpaceStation" />.
/// </summary>
public static class SpaceStationController
{
    /// <summary>
    /// Fetches a list of all available <see cref="SpaceStation" /> information available.
    /// </summary>
    [ProducesResponseType<List<SpaceStation>>(StatusCodes.Status200OK)]
    public static async Task<IResult> Index(HttpContext context, IStore<SpaceStation, int> store)
    {
        var stations = await store.AllAsync(context.RequestAborted);

        return Results.Ok(stations);
    }

    /// <summary>
    /// Fetches a specific <see cref="SpaceStation" /> identified by <paramref name="index" />.
    /// </summary>
    [ProducesResponseType<SpaceStation>(StatusCodes.Status200OK)]
    public static async Task<IResult> Show(HttpContext context, IStore<SpaceStation, int> store, [FromRoute] int index)
    {
        var station = await store.GetAsync(index, context.RequestAborted);

        if (station is null)
            return Results.NotFound();

        return Results.Ok(station);
    }
}
