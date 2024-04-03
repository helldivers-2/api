using Helldivers.Core.Contracts.Collections;
using Helldivers.Models.V1;
using Microsoft.AspNetCore.Mvc;

namespace Helldivers.API.Controllers.V1;

/// <summary>
/// Contains API endpoints for <see cref="Dispatch" />.
/// </summary>
public static class DispatchController
{
    /// <summary>
    /// Fetches a list of all available <see cref="Dispatch" /> information available.
    /// </summary>
    [ProducesResponseType<List<Dispatch>>(StatusCodes.Status200OK)]
    public static async Task<IResult> Index(HttpContext context, IStore<Dispatch, int> store)
    {
        var dispatches = await store.AllAsync(context.RequestAborted);

        return Results.Ok(dispatches);
    }


    /// <summary>
    /// Fetches a specific <see cref="Dispatch" /> identified by <paramref name="index" />.
    /// </summary>
    [ProducesResponseType<Dispatch>(StatusCodes.Status200OK)]
    public static async Task<IResult> Show(HttpContext context, IStore<Dispatch, int> store, [FromRoute] int index)
    {
        var dispatch = await store.GetAsync(index, context.RequestAborted);
        if (dispatch is null)
            return Results.NotFound();

        return Results.Ok(dispatch);
    }
}
