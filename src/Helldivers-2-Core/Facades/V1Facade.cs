using Helldivers.Core.Contracts;
using Helldivers.Core.Contracts.Collections;
using Helldivers.Core.Mapping;
using Helldivers.Core.Mapping.V1;
using Helldivers.Models.V1;

namespace Helldivers.Core.Facades;

/// <summary>
/// Handles dispatching incoming data to all stores for V1.
/// </summary>
public sealed class V1Facade(
    IStore<War> warStore,
    WarMapper warMapper,
    IStore<Planet, int> planetStore,
    PlanetMapper planetMapper,
    IStore<Campaign, int> campaignStore,
    CampaignMapper campaignMapper,
    IStore<Assignment, long> assignmentStore,
    AssignmentMapper assignmentMapper,
    IStore<Dispatch, int> dispatchStore,
    DispatchMapper dispatchMapper,
    IStore<SpaceStation, long> spaceStationStore,
    SpaceStationMapper spaceStationMapper
)
{
    /// <see cref="IStore{T,TKey}.SetStore" />
    public async ValueTask UpdateStores(MappingContext context)
    {
        // TODO: map warId

        await UpdatePlanetStore(context);
        // Some mappers need access to the list of planets, so we fetch it from the freshly-mapped store.
        var planets = await planetStore.AllAsync();

        await UpdateWarStore(context, planets);
        await UpdateCampaignStore(context, planets);
        await UpdateAssignmentsStore(context);
        await UpdateDispatchStore(context);
        await UpdateSpaceStationStore(context, planets);
    }

    private async ValueTask UpdateWarStore(MappingContext context, List<Planet> planets)
    {
        var war = warMapper.MapToV1(context, planets);

        await warStore.SetStore(war);
    }

    private async ValueTask UpdatePlanetStore(MappingContext context)
    {
        var planets = planetMapper.MapToV1(context).ToList();

        await planetStore.SetStore(planets);
    }

    private async ValueTask UpdateCampaignStore(MappingContext context, List<Planet> planets)
    {
        var campaigns = campaignMapper.MapToV1(context, planets).ToList();

        await campaignStore.SetStore(campaigns);
    }

    private async ValueTask UpdateAssignmentsStore(MappingContext context)
    {
        var assignments = assignmentMapper
            .MapToV1(context)
            .ToList();

        await assignmentStore.SetStore(assignments);
    }

    private async ValueTask UpdateDispatchStore(MappingContext context)
    {
        var dispatches = dispatchMapper
            .MapToV1(context)
            .OrderByDescending(dispatch => dispatch.Id)
            .ToList();

        await dispatchStore.SetStore(dispatches);
    }

    private async ValueTask UpdateSpaceStationStore(MappingContext context, List<Planet> planets)
    {
        var spaceStations = spaceStationMapper
            .MapToV1(context, planets)
            .ToList();

        await spaceStationStore.SetStore(spaceStations);
    }
}
