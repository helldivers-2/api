using Helldivers.Models.V1.SpaceStations;

namespace Helldivers.Models.V1;

/// <summary>
/// Represents a Super Earth Democracy Space Station.
/// </summary>
/// <param name="Id32">The unique identifier of the station.</param>
/// <param name="Planet">The planet it's currently orbiting.</param>
/// <param name="ElectionEnd">When the election for the next planet will end.</param>
/// <param name="Flags">A set of flags, purpose currently unknown.</param>
public sealed record SpaceStation(
    long Id32,
    Planet Planet,
    DateTime ElectionEnd,
    int Flags,
    List<TacticalAction> TacticalActions
);
