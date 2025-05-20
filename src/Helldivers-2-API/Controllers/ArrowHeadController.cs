using Helldivers.Core.Storage.ArrowHead;
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
    [ProducesResponseType<WarId>(StatusCodes.Status200OK)]
    public static async Task<IResult> WarId(HttpContext context, ArrowHeadStore store)
    {
        var warId = await store.GetWarId(context.RequestAborted);

        return Results.Bytes(warId, contentType: "application/json");
    }

    /// <summary>
    /// Get a snapshot of the current war status.
    /// </summary>
    [ProducesResponseType<WarStatus>(StatusCodes.Status200OK)]
    public static async Task<IResult> Status(HttpContext context, ArrowHeadStore store)
    {
        var status = await store.GetWarStatus(context.RequestAborted);

        return Results.Bytes(status, contentType: "application/json");
    }

    /// <summary>
    /// Gets the current war info.
    /// </summary>
    [ProducesResponseType<WarInfo>(StatusCodes.Status200OK)]
    public static async Task<IResult> WarInfo(HttpContext context, ArrowHeadStore store)
    {
        var info = await store.GetWarInfo(context.RequestAborted);

        return Results.Bytes(info, contentType: "application/json");
    }

    /// <summary>
    /// Gets the current war info.
    /// </summary>
    [ProducesResponseType<WarSummary>(StatusCodes.Status200OK)]
    public static async Task<IResult> Summary(HttpContext context, ArrowHeadStore store)
    {
        var summary = await store.GetWarSummary(context.RequestAborted);

        return Results.Bytes(summary, contentType: "application/json");
    }

    /// <summary>
    /// Retrieves a list of news messages from Super Earth.
    /// </summary>
    [ProducesResponseType<List<NewsFeedItem>>(StatusCodes.Status200OK)]
    public static async Task<IResult> NewsFeed(HttpContext context, ArrowHeadStore store)
    {
        var items = await store.GetNewsFeeds(context.RequestAborted);

        return Results.Bytes(items, contentType: "application/json");
    }

    /// <summary>
    /// Retrieves a list of currently active assignments (like Major Orders).
    /// </summary>
    [ProducesResponseType<List<Assignment>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public static async Task<IResult> Assignments(HttpContext context, ArrowHeadStore store)
    {
        var assignments = await store.GetAssignments(context.RequestAborted);

        return Results.Bytes(assignments, contentType: "application/json");
    }

    /// <summary>
    /// Fetches THE specific <see cref="SpaceStation" /> (749875195).
    /// </summary>
    [ProducesResponseType<List<Assignment>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public static async Task<IResult> SpaceStation(HttpContext context, ArrowHeadStore store, [FromRoute] long index)
    {
        var spaceStation = await store.GetSpaceStation(index, context.RequestAborted);
        if (spaceStation is { } bytes)
            return Results.Bytes(bytes, contentType: "application/json");

        return Results.NotFound();
    }
}
