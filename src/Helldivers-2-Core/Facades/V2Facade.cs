using Helldivers.Core.Contracts.Collections;
using Helldivers.Core.Mapping;
using Helldivers.Core.Mapping.V2;
using Helldivers.Models.V1;
using Helldivers.Models.V2;
using Dispatch = Helldivers.Models.V2.Dispatch;

namespace Helldivers.Core.Facades;

/// <summary>
/// Handles dispatching incoming data to all stores for V2.
/// </summary>
public sealed class V2Facade(
    IStore<Dispatch, int> dispatchStore,
    DispatchMapper dispatchMapper,
    IStore<Planet, int> planetStore,
    IStore<SpaceStation, long> spaceStationStore,
    SpaceStationMapper spaceStationMapper
)
{
    /// <see cref="IStore{T,TKey}.SetStore" />
    public async ValueTask UpdateStores(MappingContext context)
    {
        await UpdateDispatchStore(context);

        // Some mappers need access to the list of planets, so we fetch it from the freshly-mapped store.
        var planets = await planetStore.AllAsync();
        await UpdateSpaceStationStore(context, planets);
    }

    private async ValueTask UpdateDispatchStore(MappingContext context)
    {
        var dispatches = dispatchMapper
            .MapToV2(context)
            .OrderByDescending(dispatch => dispatch.Id)
            .ToList();

        await dispatchStore.SetStore(dispatches);
    }

    private async ValueTask UpdateSpaceStationStore(MappingContext context, List<Planet> planets)
    {
        var spaceStations = spaceStationMapper
            .MapToV2(context, planets)
            .ToList();

        await spaceStationStore.SetStore(spaceStations);
    }
}
