using Helldivers.Core.Contracts.Collections;
using Helldivers.Models.V2;

namespace Helldivers.Core.Storage.V2;

/// <inheritdoc cref="IStore{T,TKey}" />
public sealed class DispatchStore : StoreBase<Dispatch, int>
{
    /// <inheritdoc />
    protected override bool GetAsyncPredicate(Dispatch dispatch, int key)
        => dispatch.Id == key;
}
