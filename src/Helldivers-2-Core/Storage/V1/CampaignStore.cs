using Helldivers.Core.Contracts.Collections;
using Helldivers.Models.V1;

namespace Helldivers.Core.Storage.V1;

/// <inheritdoc cref="IStore{T,TKey}" />
public sealed class CampaignStore : StoreBase<Campaign, int>
{
    /// <inheritdoc />
    protected override bool GetAsyncPredicate(Campaign campaign, int key)
        => campaign.Id == key;
}
