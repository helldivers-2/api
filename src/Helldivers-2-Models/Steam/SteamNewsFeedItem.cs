namespace Helldivers.Models.Steam;

/// <summary>
/// Represents a single item in the Steam news feed response.
/// </summary>
/// <param name="Title">The title </param>
/// <param name="Url">The URL to the full article on Steam</param>
/// <param name="Author">The name of the author that released the news item.</param>
/// <param name="Contents">The contents of the news item.</param>
/// <param name="Date">When the news item was published.</param>
public sealed record SteamNewsFeedItem(
    string Gid,
    string Title,
    string Url,
    string Author,
    string Contents,
    long Date
);
