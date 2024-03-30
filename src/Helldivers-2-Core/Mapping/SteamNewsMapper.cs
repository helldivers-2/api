using Helldivers.Models.Domain;
using Helldivers.Models.Steam;

namespace Helldivers.Core.Mapping;

/// <summary>
/// Maps <see cref="SteamNewsFeedItem" /> from Steam's models.
/// </summary>
public static class SteamNewsMapper
{
    /// <summary>
    /// Maps information from Steam's API into a domain <see cref="SteamNews" />.
    /// </summary>
    public static SteamNews MapToDomain(SteamNewsFeedItem item)
    {
        return new SteamNews(
            Title: item.Title,
            Url: item.Url,
            Author: item.Author,
            Content: item.Contents,
            PublishedAt: DateTime.UnixEpoch.AddSeconds(item.Date)
        );
    }
}
