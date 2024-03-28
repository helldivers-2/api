using Helldivers.Core;
using Helldivers.Models.ArrowHead;
using Microsoft.AspNetCore.Mvc;

namespace Helldivers.API.Controllers;

/// <summary>
/// Contains API endpoints to mimic the endpoints exposed by the official ArrowHead API.
/// </summary>
public static class ArrowHeadController
{
    /// <summary>
    /// Returns the currently active war season ID.
    /// </summary>
    /// <response code="503">Thrown when the server hasn't finished it's sync and has no information.</response>
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public static IResult WarId(WarSnapshot snapshot)
    {
        if (string.IsNullOrWhiteSpace(snapshot.Season))
            return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);

        return Results.Ok(snapshot.Season);
    }

    /// <summary>
    /// Get a snapshot of the current war status.
    /// </summary>
    /// <response code="503">Thrown when the server hasn't finished it's sync and has no information.</response>
    [ProducesResponseType<WarStatus>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public static IResult Status(WarSnapshot snapshot)
    {
        if (snapshot.ArrowHeadWarStatus is null)
            return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);

        if (snapshot.ArrowHeadWarStatus.Get() is { } status)
            return Results.Ok(status);

        return Results.BadRequest();
    }

    /// <summary>
    /// Gets the current war info.
    /// </summary>
    /// <response code="503">Thrown when the server hasn't finished it's sync and has no information.</response>
    [ProducesResponseType<WarInfo>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public static IResult WarInfo(WarSnapshot snapshot)
    {
        if (snapshot.ArrowHeadWarInfo is null)
            return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);

        return Results.Ok(snapshot.ArrowHeadWarInfo);
    }

    /// <summary>
    /// Gets the current war info.
    /// </summary>
    /// <response code="503">Thrown when the server hasn't finished it's sync and has no information.</response>
    [ProducesResponseType<WarInfo>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public static IResult Summary(WarSnapshot snapshot)
    {
        if (snapshot.ArrowHeadWarSummary is null)
            return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);

        return Results.Ok(snapshot.ArrowHeadWarSummary);
    }

    /// <summary>
    /// Retrieves a list of news messages from Super Earth.
    /// </summary>
    /// <response code="503">Thrown when the server hasn't finished it's sync and has no information.</response>
    [ProducesResponseType<List<NewsFeedItem>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public static IResult NewsFeed(WarSnapshot snapshot)
    {
        if (snapshot.ArrowHeadNewsFeed is null)
            return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);

        if (snapshot.ArrowHeadNewsFeed.Get() is { } feed)
            return Results.Ok(feed);

        return Results.BadRequest();
    }

    /// <summary>
    /// Retrieves a list of currently active assignments (like Major Orders).
    /// </summary>
    /// <response code="503">Thrown when the server hasn't finished it's sync and has no information.</response>
    [ProducesResponseType<List<Assignment>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public static IResult Assignment(WarSnapshot snapshot)
    {
        if (snapshot.ArrowHeadAssignments is null)
            return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);

        if (snapshot.ArrowHeadAssignments.Get() is { } assignments)
            return Results.Ok(assignments);

        return Results.BadRequest();
    }
}
