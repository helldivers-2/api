using Helldivers.Models.Steam;
using Helldivers.Models.V1;

namespace Helldivers.Core.Mapping.Steam;

/// <summary>
/// Maps <see cref="SteamNewsFeedItem" /> from Steam's models.
/// </summary>
public sealed class SteamNewsMapper
{
    /// <summary>
    /// Maps information from Steam's API into a V1 <see cref="SteamNews" />.
    /// </summary>
    public SteamNews MapToDomain(SteamNewsFeedItem item)
    {
        return new SteamNews(
            Id: item.Gid,
            Title: item.Title,
            Url: item.Url,
            Author: item.Author,
            Content: item.Contents,
            PublishedAt: DateTime.UnixEpoch.AddSeconds(item.Date)
        );
    }
}
