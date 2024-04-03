using Helldivers.Core.Contracts;
using Helldivers.Models.V1;
using Microsoft.AspNetCore.Mvc;

namespace Helldivers.API.Controllers.V1;

/// <summary>
/// Contains API endpoints for <see cref="War" />.
/// </summary>
public static class WarController
{
    /// <summary>
    /// Gets the current <see cref="War" /> state.
    /// </summary>
    [ProducesResponseType<War>(StatusCodes.Status200OK)]
    public static async Task<IResult> Show(HttpContext context, IStore<War> store)
    {
        var war = await store.Get(context.RequestAborted);

        return Results.Ok(war);
    }
}
