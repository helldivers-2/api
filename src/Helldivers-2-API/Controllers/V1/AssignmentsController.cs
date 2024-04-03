using Helldivers.Core.Contracts.Collections;
using Helldivers.Models.V1;
using Microsoft.AspNetCore.Mvc;

namespace Helldivers.API.Controllers.V1;

/// <summary>
/// Contains API endpoints for <see cref="Assignment" />.
/// </summary>
public static class AssignmentsController
{
    /// <summary>
    /// Fetches a list of all available <see cref="Assignment" /> information available.
    /// </summary>
    [ProducesResponseType<List<Assignment>>(StatusCodes.Status200OK)]
    public static async Task<IResult> Index(HttpContext context, IStore<Assignment, long> store)
    {
        var assignments = await store.AllAsync(context.RequestAborted);

        return Results.Ok(assignments);
    }


    /// <summary>
    /// Fetches a specific <see cref="Assignment" /> identified by <paramref name="index" />.
    /// </summary>
    [ProducesResponseType<Assignment>(StatusCodes.Status200OK)]
    public static async Task<IResult> Show(HttpContext context, IStore<Assignment, long> store, [FromRoute] long index)
    {
        var assignment = await store.GetAsync(index, context.RequestAborted);
        if (assignment is null)
            return Results.NotFound();

        return Results.Ok(assignment);
    }
}
