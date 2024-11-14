namespace Helldivers.Models.ArrowHead.Status;

/// <summary>
/// Represents one of the space stations as passed from the ArrowHead API.
/// </summary>
/// <param name="Id32">The unique identifier of the station.</param>
/// <param name="PlanetIndex">The id of the planet it's currently orbiting</param>
/// <param name="CurrentElectionEndWarTime">When the election for the next planet will end (in seconds relative to game start).</param>
/// <param name="Flags">A set of flags, purpose currently unknown.</param>
public sealed record SpaceStation(
    long Id32,
    int PlanetIndex,
    // TODO PlanetActiveEffects
    ulong CurrentElectionEndWarTime,
    int Flags
);
