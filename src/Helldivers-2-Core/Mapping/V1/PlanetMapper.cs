﻿using Helldivers.Models;
using Helldivers.Models.ArrowHead;
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
    /// see <see cref="MapToV1(Helldivers.Models.ArrowHead.WarInfo,Helldivers.Models.ArrowHead.WarStatus,Helldivers.Models.ArrowHead.WarSummary)" />
    /// </summary>
    public IEnumerable<Planet> MapToV1(WarInfo warInfo, WarStatus warStatus, WarSummary summary)
    {
        foreach (var info in warInfo.PlanetInfos)
        {
            var status = warStatus.PlanetStatus.First(status => status.Index == info.Index);
            var stats = summary.PlanetsStats.FirstOrDefault(stats => stats.PlanetIndex == info.Index);
            var @event = warStatus.PlanetEvents.FirstOrDefault(@event => @event.PlanetIndex == info.Index);

            yield return MapToV1(info, status, @event, stats);
        }
    }

    /// <summary>
    /// Merges all ArrowHead data points on planets into a single <see cref="Planet" /> object.
    /// </summary>
    public Planet MapToV1(PlanetInfo info, PlanetStatus status, PlanetEvent? @event, PlanetStats? stats)
    {
        Static.Planets.TryGetValue(info.Index, out var name);
        Static.Factions.TryGetValue(info.InitialOwner, out var initialOwner);
        Static.Factions.TryGetValue(status.Owner, out var currentOwner);

        return new Planet(
            Index: info.Index,
            Name: name ?? string.Empty,
            Sector: Static.Sectors.First(sector => sector.Value.Contains(info.Index)).Key,
            Hash: info.SettingsHash,
            Position: new Position(info.Position.X, info.Position.Y),
            Waypoints: info.Waypoints.ToList(),
            MaxHealth: info.MaxHealth,
            Health: status.Health,
            Disabled: info.Disabled,
            InitialOwner: initialOwner ?? string.Empty,
            CurrentOwner: currentOwner ?? string.Empty,
            RegenPerSecond: status.RegenPerSecond,
            Event: MapToV1(@event),
            Statistics: statisticsMapper.MapToV1(stats, status)
        );
    }

    private Event? MapToV1(PlanetEvent? @event)
    {
        if (@event is null)
            return null;

        Static.Factions.TryGetValue(@event.Race, out var faction);

        return new Event(
            Id: @event.Id,
            EventType: @event.EventType,
            Faction: faction ?? string.Empty,
            Health: @event.Health,
            StartTime: DateTime.UnixEpoch.AddSeconds(@event.StartTime),
            EndTime: DateTime.UnixEpoch.AddSeconds(@event.ExpireTime),
            CampaignId: @event.CampaignId,
            JointOperationIds: @event.JointOperationIds
        );
    }
}
