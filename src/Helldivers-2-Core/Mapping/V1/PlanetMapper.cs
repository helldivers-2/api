using Helldivers.Models;
using Helldivers.Models.ArrowHead.Info;
using Helldivers.Models.ArrowHead.Status;
using Helldivers.Models.ArrowHead.Summary;
using Helldivers.Models.V1;
using Helldivers.Models.V1.Planets;

namespace Helldivers.Core.Mapping.V1;

/// <summary>
/// Handles mapping for <see cref="Planet" />.
/// </summary>
public sealed class PlanetMapper(StatisticsMapper statisticsMapper)
{
    /// <summary>
    /// Maps all planet information into a list of <see cref="Planet" /> objects.
    /// see <see cref="MapToV1(MappingContext, PlanetInfo, PlanetStatus, PlanetEvent, PlanetStats, List{int})" />
    /// </summary>
    public IEnumerable<Planet> MapToV1(MappingContext context)
    {
        foreach (var info in context.WarInfo.PlanetInfos)
        {
            var status = context.InvariantWarStatus.PlanetStatus.First(status => status.Index == info.Index);
            var stats = context.WarSummary.PlanetsStats.FirstOrDefault(stats => stats.PlanetIndex == info.Index);
            var @event = context.InvariantWarStatus.PlanetEvents.FirstOrDefault(@event => @event.PlanetIndex == info.Index);
            var attacking = context.InvariantWarStatus.PlanetAttacks
                .Where(attack => attack.Source == info.Index)
                .Select(attack => attack.Target)
                .ToList();

            yield return MapToV1(context, info, status, @event, stats, attacking);
        }
    }

    /// <summary>
    /// Merges all ArrowHead data points on planets into a single <see cref="Planet" /> object.
    /// </summary>
    private Planet MapToV1(MappingContext context, PlanetInfo info, PlanetStatus status, PlanetEvent? @event, PlanetStats? stats, List<int> attacking)
    {
        Static.Planets.TryGetValue(info.Index, out var planet);
        Static.Factions.TryGetValue(info.InitialOwner, out var initialOwner);
        Static.Factions.TryGetValue(status.Owner, out var currentOwner);

        var regionStates = context.InvariantWarStatus.PlanetRegions.Where(region => region.PlanetIndex == info.Index).ToList();
        var regions = context.WarInfo.PlanetRegions
            .Where(region => region.PlanetIndex == info.Index)
            .Select(region => (region, regionStates.FirstOrDefault(state => state.RegionIndex == region.RegionIndex)))
            .ToList();

        var (name, sector, biomeKey, environmentals) = planet;

        return new Planet(
            Index: info.Index,
            Name: name,
            Sector: sector,
            Biome: Static.Biomes[biomeKey],
            Hazards: environmentals.Select(environmental => Static.Environmentals[environmental]).ToList(),
            Hash: info.SettingsHash,
            Position: new Position(info.Position.X, info.Position.Y),
            Waypoints: info.Waypoints.ToList(),
            MaxHealth: info.MaxHealth,
            Health: status.Health,
            Disabled: info.Disabled,
            InitialOwner: initialOwner ?? string.Empty,
            CurrentOwner: currentOwner ?? string.Empty,
            RegenPerSecond: status.RegenPerSecond,
            Event: MapToV1(@event, context),
            Statistics: statisticsMapper.MapToV1(stats, status),
            Attacking: attacking,
            Regions: regions.Select(region => MapToV1(region.region, region.Item2, context)).ToList()
        );
    }

    private Event? MapToV1(PlanetEvent? @event, MappingContext context)
    {
        if (@event is null)
            return null;

        Static.Factions.TryGetValue(@event.Race, out var faction);

        return new Event(
            Id: @event.Id,
            EventType: @event.EventType,
            Faction: faction ?? string.Empty,
            Health: @event.Health,
            MaxHealth: @event.MaxHealth,
            StartTime: context.RelativeGameStart.AddSeconds(@event.StartTime),
            EndTime: context.RelativeGameStart.AddSeconds(@event.ExpireTime),
            CampaignId: @event.CampaignId,
            JointOperationIds: @event.JointOperationIds
        );
    }

    private Region MapToV1(Models.ArrowHead.Info.PlanetRegion region, Models.ArrowHead.Status.PlanetRegionStatus? status, MappingContext context)
    {
        string? owner = null;
        (string Name, string? Description)? planetRegion = null;
        if (status is { Owner: var faction })
            Static.Factions.TryGetValue(faction, out owner);

        if (Static.PlanetRegion.ContainsKey(region.SettingsHash))
            planetRegion = Static.PlanetRegion[region.SettingsHash];

        return new Region(
            Id: region.RegionIndex,
            Hash: region.SettingsHash,
            Name: planetRegion?.Name,
            Description: planetRegion?.Description,
            Health: status?.Health,
            MaxHealth: region.MaxHealth,
            Size: (RegionSize)region.RegionSize,
            RegenPerSecond: status?.RegerPerSecond,
            AvailabilityFactor: status?.AvailabilityFactor,
            IsAvailable: status?.IsAvailable ?? false,
            Players: status?.Players ?? 0
        );
    }
}
