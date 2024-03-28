using Helldivers.Models;
using Helldivers.Models.ArrowHead.Info;
using Helldivers.Models.ArrowHead.Status;
using Helldivers.Models.ArrowHead.Summary;
using Helldivers.Models.Domain;
using Helldivers.Models.Domain.War;

namespace Helldivers.Core.Mapping;

/// <summary>
/// Maps <see cref="Planet" /> from ArrowHead's models.
/// </summary>
public static class PlanetMapper
{
    /// <summary>
    /// Maps information from ArrowHead's API into a domain <see cref="Planet" />.
    /// </summary>
    public static Planet MapToDomain(PlanetInfo info, PlanetStatus status, PlanetStats? stats)
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
            Players: status.Players,
            Statistics: new Statistics(
                MissionsWon: stats?.MissionsWon ?? 0,
                MissionsLost: stats?.MissionsLost ?? 0,
                MissionTime: stats?.MissionTime ?? 0,
                TerminidKills: stats?.BugKills ?? 0,
                AutomatonKills: stats?.AutomatonKills ?? 0,
                IlluminateKills: stats?.IlluminateKills ?? 0,
                BulletsFired: stats?.BulletsFired ?? 0,
                BulletsHit: stats?.BulletsHit ?? 0,
                TimePlayed: stats?.TimePlayed ?? 0,
                Deaths: stats?.Deaths ?? 0,
                Revives: stats?.Revives ?? 0,
                Friendlies: stats?.Friendlies ?? 0,
                MissionSuccessRate: stats?.MissionSuccessRate ?? 0,
                Accuracy: stats?.Accurracy ?? 0
            )
        );
    }
}
