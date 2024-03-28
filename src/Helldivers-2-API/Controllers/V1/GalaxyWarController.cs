using Helldivers.Core;
using Helldivers.Models.Domain;

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
    public static IResult Show(WarSnapshot snapshot)
    {
        if (snapshot.GalacticWar is { } war)
            return Results.Ok(war);

        return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }
}
