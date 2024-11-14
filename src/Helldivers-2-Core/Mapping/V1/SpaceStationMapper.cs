using Helldivers.Models;
using Helldivers.Models.V1;

namespace Helldivers.Core.Mapping.V1;

/// <summary>
/// Handles mapping for <see cref="SpaceStation" />.
/// </summary>
public sealed class SpaceStationMapper
{
    /// <summary>
    /// Maps all space stations in the <see cref="MappingContext" />.
    /// </summary>
    /// <param name="context">The mapping context containing the invariant war status and other relevant data.</param>
    /// <param name="planets">The list of planets to map with.</param>
    /// <returns>An enumerable list of space stations mapped to the V1 model.</returns>
    public IEnumerable<SpaceStation> MapToV1(MappingContext context, List<Planet> planets)
    {
        foreach (var station in context.InvariantWarStatus.SpaceStations)
            yield return Map(context, station, planets);
    }

    private SpaceStation Map(MappingContext context, Helldivers.Models.ArrowHead.Status.SpaceStation raw, List<Planet> planets)
    {
        var planet = planets.First(p => p.Index == raw.PlanetIndex);

        return new SpaceStation(
            Id32: raw.Id32,
            Planet: planet,
            ElectionEnd: context.RelativeGameStart.AddSeconds(raw.CurrentElectionEndWarTime),
            Flags: raw.Flags
        );
    }
}
