namespace Helldivers.Models.Steam;

/// <summary>
/// Represents the response of the Steam news feed API call.
/// </summary>
/// <param name="AppId">The appid of Helldivers 2 on Steam.</param>
/// <param name="NewsItems">A list of newsfeed items.</param>
public sealed record SteamAppNews(
    long AppId,
    List<SteamNewsFeedItem> NewsItems
);
