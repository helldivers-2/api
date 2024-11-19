using Helldivers.Core.Facades;
using Helldivers.Core.Mapping;
using Helldivers.Core.Storage.ArrowHead;
using Helldivers.Models;
using Helldivers.Models.Steam;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Helldivers.Core;

/// <summary>
/// Rather than having the sync service be aware of all mappings and storage versions,
/// this facade class handles dispatching incoming data to the correct underlying stores.
/// </summary>
public sealed class StorageFacade(ArrowHeadStore arrowHead, SteamFacade steam, V1Facade v1)
{
    /// <summary>
    /// Updates all stores that rely on <see cref="SteamNewsFeed" />.
    /// </summary>
    public ValueTask UpdateStores(SteamNewsFeed feed)
        => steam.UpdateStores(feed);

    /// <summary>
    /// Updates all stores that rely on ArrowHead's models.
    /// </summary>
    public async ValueTask UpdateStores(Memory<byte> rawWarId, Memory<byte> rawWarInfo,
        Dictionary<string, Memory<byte>> rawWarStatuses, Memory<byte> rawWarSummary,
        Dictionary<string, Memory<byte>> rawNewsFeeds, Dictionary<string, Memory<byte>> rawAssignments,
        Dictionary<string, Memory<byte>> rawStations)
    {
        arrowHead.UpdateRawStore(
            rawWarId,
            rawWarInfo,
            rawWarSummary,
            rawWarStatuses,
            rawNewsFeeds,
            rawAssignments,
            rawStations
        );

        var warId = DeserializeOrThrow(rawWarId, ArrowHeadSerializerContext.Default.WarId);
        var warInfo = DeserializeOrThrow(rawWarInfo, ArrowHeadSerializerContext.Default.WarInfo);
        var warStatuses = rawWarStatuses.ToDictionary(
            pair => pair.Key,
            pair => DeserializeOrThrow(pair.Value, ArrowHeadSerializerContext.Default.WarStatus)
        );
        var warSummary = DeserializeOrThrow(rawWarSummary, ArrowHeadSerializerContext.Default.WarSummary);
        var newsFeeds = rawNewsFeeds.ToDictionary(
            pair => pair.Key,
            pair => DeserializeOrThrow(pair.Value, ArrowHeadSerializerContext.Default.ListNewsFeedItem)
        );
        var assignments = rawAssignments.ToDictionary(
            pair => pair.Key,
            pair => DeserializeOrThrow(pair.Value, ArrowHeadSerializerContext.Default.ListAssignment)
        );
        var spaceStations = rawAssignments.ToDictionary(
            pair => pair.Key,
            pair => DeserializeOrThrow(pair.Value, ArrowHeadSerializerContext.Default.ListSpaceStation)
        );

        var context = new MappingContext(
            warId,
            warInfo,
            warStatuses,
            warSummary,
            newsFeeds,
            assignments,
            spaceStations
        );

        await v1.UpdateStores(context);
    }

    private T DeserializeOrThrow<T>(Memory<byte> memory, JsonTypeInfo<T> typeInfo)
        => JsonSerializer.Deserialize(memory.Span, typeInfo) ?? throw new InvalidOperationException();
}
