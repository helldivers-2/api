using Helldivers.Core;

namespace Helldivers.API.Controllers;

/// <summary>
/// Controller used for health checks on the API.
/// </summary>
public static class HealthController
{
    /// <summary>
    /// 
    /// </summary>
    /// <response code="204">Server has not finished synchronizing, API may be unavailable.</response>
    /// <response code="200">Server has finished synchronizing.</response>
    public static IResult Show(WarSnapshot snapshot)
    {
        if (snapshot.ArrowHeadWarInfo is null)
            return Results.NoContent();

        return Results.Ok();
    }
}
