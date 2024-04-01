using Helldivers.Core.Contracts;
using Helldivers.Core.Contracts.Collections;
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
    DispatchMapper dispatchMapper
)
{
    /// <see cref="IStore{T,TKey}.SetStore" />
    public async ValueTask UpdateStores(Models.ArrowHead.WarId warId, Models.ArrowHead.WarInfo warInfo, Dictionary<string, Models.ArrowHead.WarStatus> warStatuses, Models.ArrowHead.WarSummary warSummary, Dictionary<string, List<Models.ArrowHead.NewsFeedItem>> newsFeeds, Dictionary<string, List<Models.ArrowHead.Assignment>> assignments)
    {
        // Fetch a WarStatus for mapping that don't need localized data.
        var invariantStatus = warStatuses.FirstOrDefault().Value;

        await UpdateWarStore(warInfo, invariantStatus, warSummary);
        await UpdatePlanetStore(warInfo, invariantStatus, warSummary);

        // Some mappers need access to the list of planets, so we fetch it from the freshly-mapped store.
        var planets = await planetStore.AllAsync();

        await UpdateCampaignStore(invariantStatus, planets);
        await UpdateAssignmentsStore(assignments);
        await UpdateDispatchStore(warInfo, newsFeeds);
    }

    private async ValueTask UpdateWarStore(Models.ArrowHead.WarInfo info, Models.ArrowHead.WarStatus status, Models.ArrowHead.WarSummary summary)
    {
        var war = warMapper.MapToV1(info, status, summary);

        await warStore.SetStore(war);
    }

    private async ValueTask UpdatePlanetStore(Models.ArrowHead.WarInfo warInfo, Models.ArrowHead.WarStatus warStatus, Models.ArrowHead.WarSummary summary)
    {
        var planets = planetMapper.MapToV1(warInfo, warStatus, summary).ToList();

        await planetStore.SetStore(planets);
    }

    private async ValueTask UpdateCampaignStore(Models.ArrowHead.WarStatus status, List<Planet> planets)
    {
        var campaigns = status
            .Campaigns
            .Select(campaign => campaignMapper.MapToV1(campaign, planets))
            .ToList();

        await campaignStore.SetStore(campaigns);
    }

    private async ValueTask UpdateAssignmentsStore(Dictionary<string, List<Models.ArrowHead.Assignment>> translations)
    {
        var assignments = assignmentMapper
            .MapToV1(translations)
            .ToList();

        await assignmentStore.SetStore(assignments);
    }

    private async ValueTask UpdateDispatchStore(Models.ArrowHead.WarInfo info, Dictionary<string, List<Models.ArrowHead.NewsFeedItem>> translations)
    {
        var dispatches = dispatchMapper
            .MapToV1(info, translations)
            .ToList();

        await dispatchStore.SetStore(dispatches);
    }
}
