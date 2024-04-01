using Helldivers.Core.Contracts;
using Helldivers.Core.Storage.ArrowHead;
using Helldivers.Models.ArrowHead;

namespace Helldivers.Core.Facades;

/// <summary>
/// Handles dispatching incoming data to all stores for ArrowHead.
/// </summary>
public sealed class ArrowHeadFacade(
    IStore<WarId> warIdStore,
    IStore<WarInfo> warInfoStore,
    WarStatusStore warStatusStore,
    IStore<WarSummary> warSummaryStore,
    NewsFeedStore newsFeedStore,
    AssignmentStore assignmentStore
)
{
    /// <see cref="IStore{T}.SetStore" />
    public async ValueTask UpdateStores(WarId warId, WarInfo warInfo, Dictionary<string, WarStatus> warStatuses, WarSummary warSummary, Dictionary<string, List<NewsFeedItem>> newsFeeds, Dictionary<string, List<Assignment>> assignments)
    {
        await warIdStore.SetStore(warId);
        await warInfoStore.SetStore(warInfo);
        await warStatusStore.SetStore(warStatuses);
        await warSummaryStore.SetStore(warSummary);
        await newsFeedStore.SetStore(newsFeeds);
        await assignmentStore.SetStore(assignments);
    }
}
