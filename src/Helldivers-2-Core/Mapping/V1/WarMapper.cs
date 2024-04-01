using Helldivers.Models.ArrowHead;
using Helldivers.Models.V1;

namespace Helldivers.Core.Mapping.V1;

public sealed class WarMapper(StatisticsMapper statisticsMapper)
{
    /// <summary>
    /// Handles mapping <see cref="War" /> to V1.
    /// </summary>
    public War MapToV1(WarInfo info, WarStatus status, WarSummary summary)
    {
        return new War(
            Started: DateTime.UnixEpoch.AddSeconds(info.StartDate),
            Ended: DateTime.UnixEpoch.AddSeconds(info.EndDate),
            Now: DateTime.UnixEpoch.AddSeconds(status.Time),
            ClientVersion: info.MinimumClientVersion,
            Factions: [],
            ImpactMultiplier: status.ImpactMultiplier,
            Statistics: statisticsMapper.MapToV1(summary.GalaxyStats)
        );
    }
}
