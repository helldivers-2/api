namespace Helldivers.Models.Domain;

/// <summary>
/// Represents an item in the news feed (shown as 'dispatch' in the game).
/// </summary>
/// <param name="Index">Numerical identifier of this message.</param>
/// <param name="PublishedAt">When this news item was published.</param>
/// <param name="Type">A numerical type, purpose unknown.</param>
/// <param name="Message">The message for this news item.</param>
public sealed record NewsItem(
    int Index,
    DateTime PublishedAt,
    int Type,
    string Message
);
