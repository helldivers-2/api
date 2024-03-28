using Helldivers.Models.ArrowHead;
using Helldivers.Models.Domain;

namespace Helldivers.Core.Mapping;

/// <summary>
/// Handles mapping <see cref="NewsFeedItem" /> into <see cref="NewsItem" />.
/// </summary>
public static class NewsItemMapper
{
    /// <summary>
    /// Maps information from the ArrowHead API onto domain <see cref="NewsItem" />.
    /// </summary>
    public static NewsItem MapToDomain(NewsFeedItem item, GalacticWar war)
    {
        return new NewsItem(
            Index: item.Id,
            PublishedAt: war.StartedAt.AddSeconds(item.Published),
            Type: item.Type,
            Message: item.Message
        );
    }
}
