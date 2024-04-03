using Helldivers.Core.Contracts.Collections;
using Helldivers.Models.V1;

namespace Helldivers.Core.Storage.V1;

/// <inheritdoc cref="IStore{T,TKey}" />
public class PlanetStore : StoreBase<Planet, int>
{
    /// <inheritdoc />
    protected override bool GetAsyncPredicate(Planet planet, int index) => planet.Index == index;
}
