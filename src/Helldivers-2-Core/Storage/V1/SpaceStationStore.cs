using Helldivers.Core.Contracts.Collections;
using Helldivers.Models.V1;

namespace Helldivers.Core.Storage.V1;

/// <inheritdoc cref="IStore{T,TKey}" />
public sealed class SpaceStationStore : StoreBase<SpaceStation, long>
{
    /// <inheritdoc />
    protected override bool GetAsyncPredicate(SpaceStation station, long index) => station.Id32 == index;
}
