namespace Helldivers.Models.Steam;

/// <summary>
/// Represents the response of the Steam news feed API call.
/// </summary>
/// <param name="AppNews">Contains the <see cref="SteamAppNews" /> object.</param>
public sealed record SteamNewsFeed(
    SteamAppNews AppNews
);
