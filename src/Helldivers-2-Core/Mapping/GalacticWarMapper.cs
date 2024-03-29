using Helldivers.Models;
using Helldivers.Models.ArrowHead;
using Helldivers.Models.Domain;
using Helldivers.Models.Domain.War;
using ArrowHeadAssignment = Helldivers.Models.ArrowHead.Assignment;

namespace Helldivers.Core.Mapping;

/// <summary>
/// Handles mapping models from ArrowHead's API (like <see cref="WarInfo" />) to domain models (like <see cref="GalacticWar" />).
/// </summary>
public static class GalacticWarMapper
{
    /// <summary>
    /// Maps information from the ArrowHead API onto domain <see cref="GalacticWar" />.
    /// </summary>
    public static GalacticWar MapToDomain(
        string season,
        WarInfo warInfo,
        WarSummary summary,
        Dictionary<string, WarStatus> warStatus,
        Dictionary<string, List<NewsFeedItem>> feed,
        Dictionary<string, List<ArrowHeadAssignment>> assignments
    )
    {
        // Choose one WarStatus as 'invariant', aka the instance we grab non-localized data from.
        var invariantStatus = warStatus.First().Value;
        var planets = MapPlanets(warInfo, invariantStatus, summary)
            .OrderBy(planet => planet.Index)
            .ToList();

        return new GalacticWar(
            WarId: warInfo.WarId,
            StartedAt: DateTime.UnixEpoch.AddSeconds(warInfo.StartDate),
            EndsAt: DateTime.UnixEpoch.AddSeconds(warInfo.EndDate),
            SnapshottedAt: DateTime.UtcNow,
            ImpactMultiplier: invariantStatus.ImpactMultiplier,
            Planets: planets,
            Factions: Static.Factions.Values.ToList(),
            Attacks: new List<Attack>(), // TODO: map attacks
            Statistics: new Statistics(
                MissionsWon: summary.GalaxyStats.MissionsWon,
                MissionsLost: summary.GalaxyStats.MissionsLost,
                MissionTime: summary.GalaxyStats.MissionTime,
                TerminidKills: summary.GalaxyStats.BugKills,
                AutomatonKills: summary.GalaxyStats.AutomatonKills,
                IlluminateKills: summary.GalaxyStats.IlluminateKills,
                BulletsFired: summary.GalaxyStats.BulletsFired,
                BulletsHit: summary.GalaxyStats.BulletsHit,
                TimePlayed: summary.GalaxyStats.TimePlayed,
                Deaths: summary.GalaxyStats.Deaths,
                Revives: summary.GalaxyStats.Revives,
                Friendlies: summary.GalaxyStats.Friendlies,
                MissionSuccessRate: summary.GalaxyStats.MissionSuccessRate,
                Accuracy: summary.GalaxyStats.Accurracy
            )
        );
    }

    private static IEnumerable<Planet> MapPlanets(WarInfo warInfo, WarStatus warStatus, WarSummary summary)
    {
        foreach (var info in warInfo.PlanetInfos)
        {
            var status = warStatus.PlanetStatus.First(status => status.Index == info.Index);
            var stats = summary.PlanetsStats.FirstOrDefault(stats => stats.PlanetIndex == info.Index);

            yield return PlanetMapper.MapToDomain(info, status, stats);
        }
    }
}
