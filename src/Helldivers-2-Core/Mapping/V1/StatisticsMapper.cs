using Helldivers.Models.V1;

namespace Helldivers.Core.Mapping.V1;

public sealed class StatisticsMapper
{
    public Statistics MapToV1(Models.ArrowHead.Summary.GalaxyStats statistics)
    {
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
            Accuracy: statistics.Accurracy
        );
    }

    public Statistics MapToV1(Models.ArrowHead.Summary.PlanetStats statistics)
    {
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
            Accuracy: statistics.Accurracy
        );
    }
}
