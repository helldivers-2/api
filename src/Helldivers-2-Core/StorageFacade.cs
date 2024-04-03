using Helldivers.Core.Facades;
using Helldivers.Models.ArrowHead;
using Helldivers.Models.Steam;

namespace Helldivers.Core;

/// <summary>
/// Rather than having the sync service be aware of all mappings and storage versions,
/// this facade class handles dispatching incoming data to the correct underlying stores.
/// </summary>
public sealed class StorageFacade(ArrowHeadFacade arrowHead, SteamFacade steam, V1Facade v1)
{
    /// <summary>
    /// Updates all stores that rely on <see cref="SteamNewsFeed" />.
    /// </summary>
    public ValueTask UpdateStores(SteamNewsFeed feed)
        => steam.UpdateStores(feed);

    /// <summary>
    /// Updates all stores that rely on ArrowHead's models.
    /// </summary>
    public async ValueTask UpdateStores(WarId warId, WarInfo warInfo, Dictionary<string, WarStatus> warStatuses, WarSummary warSummary, Dictionary<string, List<NewsFeedItem>> newsFeeds, Dictionary<string, List<Assignment>> assignments)
    {
        await arrowHead.UpdateStores(warId, warInfo, warStatuses, warSummary, newsFeeds, assignments);
        await v1.UpdateStores(warId, warInfo, warStatuses, warSummary, newsFeeds, assignments);
    }
}
