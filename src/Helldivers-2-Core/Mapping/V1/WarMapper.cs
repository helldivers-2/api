using Helldivers.Models;
using Helldivers.Models.V1;

namespace Helldivers.Core.Mapping.V1;

/// <summary>
/// Handles mapping for <see cref="War" />.
/// </summary>
public sealed class WarMapper(StatisticsMapper statisticsMapper)
{
    /// <summary>
    /// Handles mapping <see cref="War" /> to V1.
    /// </summary>
    public War MapToV1(MappingContext context, List<Planet> planets)
    {
        return new War(
            Started: DateTime.UnixEpoch.AddSeconds(context.WarInfo.StartDate),
            Ended: DateTime.UnixEpoch.AddSeconds(context.WarInfo.EndDate),
            Now: DateTime.UnixEpoch.AddSeconds(context.InvariantWarStatus.Time),
            ClientVersion: context.WarInfo.MinimumClientVersion,
            Factions: Static.Factions.Values.ToList(),
            ImpactMultiplier: context.InvariantWarStatus.ImpactMultiplier,
            Statistics: statisticsMapper.MapToV1(context.WarSummary.GalaxyStats, planets)
        );
    }
}
