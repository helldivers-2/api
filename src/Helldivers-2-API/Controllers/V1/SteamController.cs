using Helldivers.Core.Contracts.Collections;
using Helldivers.Models.V1;
using Microsoft.AspNetCore.Mvc;

namespace Helldivers.API.Controllers.V1;

/// <summary>
/// Contains API endpoints for <see cref="SteamNews" />.
/// </summary>
public static class SteamController
{
    /// <summary>
    /// Fetches the Steam newsfeed for Helldivers 2.
    /// </summary>
    /// <remarks>You can definitely get this yourself, however it's included for your convenience!</remarks>
    [ProducesResponseType<List<SteamNews>>(StatusCodes.Status200OK)]
    public static async Task<IResult> Index(HttpContext context, IStore<SteamNews, string> store)
    {
        var news = await store.AllAsync(context.RequestAborted);

        return Results.Ok(news);
    }

    /// <summary>
    /// Fetches a specific newsfeed item from the Helldivers 2 Steam newsfeed.
    /// </summary>
    /// <remarks>You can definitely get this yourself, however it's included for your convenience!</remarks>
    /// <response code="404">Returned when the key is not recognized.</response>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<List<SteamNews>>(StatusCodes.Status200OK)]
    public static async Task<IResult> Show(HttpContext context, IStore<SteamNews, string> store, [FromRoute] string gid)
    {
        var news = await store.GetAsync(gid, context.RequestAborted);

        if (news is null)
            return Results.NotFound();

        return Results.Ok(news);
    }
}
