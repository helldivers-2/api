using Helldivers.Core.Contracts.Collections;
using Helldivers.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Helldivers.API.Controllers.V1;

/// <summary>
/// Contains methods for retrieving announcements.
/// </summary>
public static class AnnouncementsController
{
    /// <summary>
    /// Gets a list of announcements made from ArrowHead to the community.
    /// This is mostly patch notes, but not guaranteed to <i>only</i> be patch notes.
    /// </summary>
    /// <response code="503">Thrown when the server hasn't finished it's sync and has no information.</response>
    [ProducesResponseType<List<SteamNews>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public static async Task<IResult> Index(HttpContext context, IStore<SteamNews, string> store)
    {
        var feed = await store.AllAsync(context.RequestAborted);

        return Results.Ok(feed);
    }
}
