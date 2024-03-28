using Helldivers.Core;
using Helldivers.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Helldivers.API.Controllers.V1;

/// <summary>
/// Contains API endpoints for the news feed (dispatches).
/// </summary>
public static class NewsFeedController
{
    /// <summary>
    /// Gets all newsfeed items.
    /// </summary>
    /// <response code="503">Thrown when the server hasn't finished it's sync and has no information.</response>
    [ProducesResponseType<List<NewsItem>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public static IResult Index(WarSnapshot snapshot)
    {
        if (snapshot.NewsFeed is null)
            return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);

        if (snapshot.NewsFeed.Get() is { } feed)
            return Results.Ok(feed);

        return Results.NotFound();
    }

    /// <summary>
    /// Returns a specific newsfeed item.
    /// </summary>
    /// <response code="503">Thrown when the server hasn't finished it's sync and has no information.</response>
    [ProducesResponseType<NewsItem>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public static IResult Show(WarSnapshot snapshot, int index)
    {
        if (snapshot.NewsFeed is null)
            return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);

        if (snapshot.NewsFeed.Get() is { } feed && feed.FirstOrDefault(i => i.Index == index) is { } item)
            return Results.Ok(item);

        return Results.NotFound();
    }
}
