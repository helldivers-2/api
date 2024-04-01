using Helldivers.Core.Contracts.Collections;
using Helldivers.Models.V1;
using Microsoft.AspNetCore.Mvc;

namespace Helldivers.API.Controllers.V1;

/// <summary>
/// Contains API endpoints for <see cref="Campaign" />.
/// </summary>
public static class CampaignsController
{
    /// <summary>
    /// Fetches a list of all available <see cref="Campaign" /> information available.
    /// </summary>
    [ProducesResponseType<List<Campaign>>(StatusCodes.Status200OK)]
    public static async Task<IResult> Index(HttpContext context, IStore<Campaign, int> store)
    {
        var campaigns = await store.AllAsync(context.RequestAborted);

        return Results.Ok(campaigns);
    }


    /// <summary>
    /// Fetches a specific <see cref="Campaign" /> identified by <paramref name="index" />.
    /// </summary>
    [ProducesResponseType<Campaign>(StatusCodes.Status200OK)]
    public static async Task<IResult> Show(HttpContext context, IStore<Campaign, int> store, [FromRoute] int index)
    {
        var campaign = await store.GetAsync(index, context.RequestAborted);
        if (campaign is null)
            return Results.NotFound();

        return Results.Ok(campaign);
    }
}
