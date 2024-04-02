using Helldivers.Models.V1;

namespace Helldivers.Core.Mapping.V1;

/// <summary>
/// Handles mapping for <see cref="Statistics" />.
/// </summary>
public sealed class StatisticsMapper
{
    /// <summary>
    /// Maps galaxy wide statistics onto V1's statistics.
    /// </summary>
    public Statistics MapToV1(Models.ArrowHead.Summary.GalaxyStats statistics, List<Planet> planets)
    {
        var playerCount = planets.Aggregate(0ul, (total, planet) => total + planet.Statistics.PlayerCount);

        return new Statistics(
            MissionsWon: statistics.MissionsWon,
            MissionsLost: statistics.MissionsLost,
            MissionTime: statistics.MissionTime,
            TerminidKills: statistics.BugKills,
            AutomatonKills: statistics.AutomatonKills,
            IlluminateKills: statistics.IlluminateKills,
            BulletsFired: statistics.BulletsFired,
            BulletsHit: statistics.BulletsHit,
            TimePlayed: statistics.TimePlayed,
            Deaths: statistics.Deaths,
            Revives: statistics.Revives,
            Friendlies: statistics.Friendlies,
            MissionSuccessRate: statistics.MissionSuccessRate,
            Accuracy: statistics.Accurracy,
            PlayerCount: playerCount
        );
    }

    /// <summary>
    /// Maps statistics of a specific planet onto V1's statistics.
    /// </summary>
    public Statistics MapToV1(Models.ArrowHead.Summary.PlanetStats? statistics, Models.ArrowHead.Status.PlanetStatus status)
    {
        return new Statistics(
            MissionsWon: statistics?.MissionsWon ?? 0,
            MissionsLost: statistics?.MissionsLost ?? 0,
            MissionTime: statistics?.MissionTime ?? 0,
            TerminidKills: statistics?.BugKills ?? 0,
            AutomatonKills: statistics?.AutomatonKills ?? 0,
            IlluminateKills: statistics?.IlluminateKills ?? 0,
            BulletsFired: statistics?.BulletsFired ?? 0,
            BulletsHit: statistics?.BulletsHit ?? 0,
            TimePlayed: statistics?.TimePlayed ?? 0,
            Deaths: statistics?.Deaths ?? 0,
            Revives: statistics?.Revives ?? 0,
            Friendlies: statistics?.Friendlies ?? 0,
            MissionSuccessRate: statistics?.MissionSuccessRate ?? 0,
            Accuracy: statistics?.Accurracy ?? 0,
            PlayerCount: status.Players
        );
    }
}
