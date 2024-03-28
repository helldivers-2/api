using Helldivers.Core;
using Helldivers.Models.Domain.War;
using Microsoft.AspNetCore.Mvc;

namespace Helldivers.API.Controllers.V1;

/// <summary>
/// Contains API endpoints related to <see cref="Planet" />s.
/// </summary>
public static class PlanetsController
{
    /// <summary>
    /// Retrieves a list of all planets currently involved in the galactic war.
    /// </summary>
    /// <response code="503">Thrown when the server hasn't finished it's sync and has no information.</response>
    [ProducesResponseType<List<Planet>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public static IResult Index(WarSnapshot snapshot)
    {
        if (snapshot.GalacticWar is { Planets: var planets })
            return Results.Ok(planets);

        return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }

    /// <summary>
    /// Retrieves a specific planet by it's planet ID.
    /// </summary>
    /// <response code="503">Thrown when the server hasn't finished it's sync and has no information.</response>
    [ProducesResponseType<Planet>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public static IResult Show(WarSnapshot snapshot, int index)
    {
        if (snapshot.GalacticWar is { Planets: var planets })
        {
            if (planets.FirstOrDefault(p => p.Index == index) is { } planet)
            {
                return Results.Ok(planet);
            }

            return Results.NotFound();
        }

        return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }

    /// <summary>
    /// Retrieves a specific planet's statistics by it's planet ID.
    /// </summary>
    /// <response code="503">Thrown when the server hasn't finished it's sync and has no information.</response>
    [ProducesResponseType<Statistics>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public static IResult ShowStatistics(WarSnapshot snapshot, int index)
    {
        if (snapshot.GalacticWar is { Planets: var planets })
        {
            if (planets.FirstOrDefault(p => p.Index == index) is { } planet)
            {
                return Results.Ok(planet.Statistics);
            }

            return Results.NotFound();
        }

        return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }
}
