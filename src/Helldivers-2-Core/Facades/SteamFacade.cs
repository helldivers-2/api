using Helldivers.Core.Contracts.Collections;
using Helldivers.Core.Mapping.Steam;
using Helldivers.Core.Mapping.V1;
using Helldivers.Models.Steam;
using Helldivers.Models.V1;

namespace Helldivers.Core.Facades;

/// <summary>
/// Handles dispatching incoming data to all stores for Steam.
/// </summary>
public sealed class SteamFacade(IStore<SteamNews, string> store, SteamNewsMapper mapper)
{
    /// <see cref="IStore{T,TKey}.SetStore" />
    public async ValueTask UpdateStores(SteamNewsFeed feed)
    {
        if (feed is { AppNews.NewsItems: var items })
        {
            await store.SetStore(items.Select(mapper.MapToDomain).ToList());
        }
    }
}
