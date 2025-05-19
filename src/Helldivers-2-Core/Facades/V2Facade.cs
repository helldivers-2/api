using Helldivers.Core.Contracts.Collections;
using Helldivers.Core.Mapping;
using Helldivers.Core.Mapping.V2;
using Helldivers.Models.V2;

namespace Helldivers.Core.Facades;

/// <summary>
/// Handles dispatching incoming data to all stores for V2.
/// </summary>
public sealed class V2Facade(
    IStore<Dispatch, int> dispatchStore,
    DispatchMapper dispatchMapper
)
{
    /// <see cref="IStore{T,TKey}.SetStore" />
    public async ValueTask UpdateStores(MappingContext context)
    {
        await UpdateDispatchStore(context);
    }

    private async ValueTask UpdateDispatchStore(MappingContext context)
    {
        var dispatches = dispatchMapper
            .MapToV2(context)
            .OrderByDescending(dispatch => dispatch.Id)
            .ToList();

        await dispatchStore.SetStore(dispatches);
    }
}
