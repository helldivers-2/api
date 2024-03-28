namespace Helldivers.Models.ArrowHead;

/// <summary>
/// Represents an item in the newsfeed of Super Earth.
/// </summary>
/// <param name="Id">The identifier of this newsfeed item.</param>
/// <param name="Published">A unix timestamp (in seconds) when this item was published.</param>
/// <param name="Type">A numerical type, purpose unknown.</param>
/// <param name="Message">The message containing a human readable text.</param>
public sealed record NewsFeedItem(
    int Id,
    long Published,
    int Type,
    string Message
);
