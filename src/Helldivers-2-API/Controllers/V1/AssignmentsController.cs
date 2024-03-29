using Helldivers.Core;
using Helldivers.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Helldivers.API.Controllers.V1;

public static class AssignmentsController
{
    /// <summary>
    /// Gets all assignments.
    /// </summary>
    /// <response code="503">Thrown when the server hasn't finished it's sync and has no information.</response>
    [ProducesResponseType<List<Assignment>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public static IResult Index(WarSnapshot snapshot)
    {
        if (snapshot.Assignments is null)
            return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);

        if (snapshot.Assignments.Get() is { } assignments)
            return Results.Ok(assignments);

        return Results.NotFound();
    }

    /// <summary>
    /// Gets a specific assignment.
    /// </summary>
    /// <response code="503">Thrown when the server hasn't finished it's sync and has no information.</response>
    [ProducesResponseType<Assignment>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public static IResult Show(WarSnapshot snapshot, long index)
    {
        if (snapshot.Assignments is null)
            return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);

        if (snapshot.Assignments.Get() is { } assignments && assignments.FirstOrDefault(a => a.Index == index) is { } assignment)
            return Results.Ok(assignment);

        return Results.NotFound();
    }
}
