using Helldivers.Core;
using Helldivers.Models.Domain;
using Helldivers.Models.Domain.War;
using Microsoft.AspNetCore.Mvc;

namespace Helldivers.API.Controllers.V1;

/// <summary>
/// Contains methods for exposing <see cref="GalacticWar" /> information on the API in version 1.
/// </summary>
public static class GalaxyWarController
{
    /// <summary>
    /// Gets the current galactic war information.
    /// </summary>
    /// <response code="503">Thrown when the server hasn't finished it's sync and has no information.</response>
    [ProducesResponseType<GalacticWar>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public static IResult Show(WarSnapshot snapshot)
    {
        if (snapshot.GalacticWar is { } war)
            return Results.Ok(war);

        return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }

    /// <summary>
    /// Gets the current war season.
    /// </summary>
    /// <response code="503">Thrown when the server hasn't finished it's sync and has no information.</response>
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public static IResult ShowWarId(WarSnapshot snapshot)
    {
        if (snapshot.GalacticWar is { WarId: var warId })
            return Results.Ok(warId);

        return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }

    /// <summary>
    /// Gets the current galactic war statistics like shots fired, mission success rate, ...
    /// </summary>
    /// <response code="503">Thrown when the server hasn't finished it's sync and has no information.</response>
    [ProducesResponseType<Statistics>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public static IResult ShowStatistics(WarSnapshot snapshot)
    {
        if (snapshot.GalacticWar is { Statistics: var statistics })
            return Results.Ok(statistics);

        return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }
}
