namespace Helldivers.Models.ArrowHead.Status;

/// <summary>
/// Represents one of the space stations as passed from the ArrowHead API.
/// </summary>
/// <param name="Id32"></param>
/// <param name="PlanetIndex"></param>
/// <param name="CurrentElectionEndWarTime"></param>
/// <param name="Flags"></param>
public sealed record SpaceStation(
    long Id32,
    int PlanetIndex,
    // TODO PlanetActiveEffects
    ulong CurrentElectionEndWarTime,
    int Flags
);
