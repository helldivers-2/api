namespace Helldivers.Models.V1;

/// <summary>
/// Represents a new article from Steam's news feed.
/// </summary>
/// <param name="Id">The identifier assigned by Steam to this news item.</param>
/// <param name="Title">The title of the Steam news item.</param>
/// <param name="Url">The URL to Steam where this news item was posted.</param>
/// <param name="Author">The author who posted this message on Steam.</param>
/// <param name="Content">The message posted by Steam, currently in Steam's weird markdown format.</param>
/// <param name="PublishedAt">When this message was posted.</param>
public sealed record SteamNews(
    string Id,
    string Title,
    string Url,
    string Author,
    string Content,
    DateTime PublishedAt
);
