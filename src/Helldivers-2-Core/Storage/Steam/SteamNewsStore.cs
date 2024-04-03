using Helldivers.Core.Contracts.Collections;
using Helldivers.Models.V1;

namespace Helldivers.Core.Storage.Steam;

/// <inheritdoc cref="IStore{T,TKey}" />
public class SteamNewsStore : StoreBase<SteamNews, string>
{
    /// <inheritdoc />
    protected override bool GetAsyncPredicate(SteamNews item, string key)
        => string.Equals(item.Id, key, StringComparison.InvariantCultureIgnoreCase);
}
