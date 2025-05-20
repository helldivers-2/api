using Helldivers.Models.ArrowHead.SpaceStations;

namespace Helldivers.Models.ArrowHead;

/// <summary>
/// Represents an assignment given from Super Earth to the Helldivers.
/// </summary>
/// <param name="Id32">The unique identifier of the station.</param>
/// <param name="PlanetIndex">The id of the planet it's currently orbiting</param>
/// <param name="LastElectionId">The id of the previous planet election.</param>
/// <param name="CurrentElectionId">The id of the current planet election.</param>
/// <param name="NextElectionId">The id of the next planet election.</param>
/// <param name="CurrentElectionEndWarTime">When the election for the next planet will end (in seconds relative to game start).</param>
/// <param name="Flags">A set of flags, purpose currently unknown.</param>
/// <param name="TacticalActions">The list of actions the space station crew can perform.</param>
public sealed record SpaceStation(
    long Id32,
    int PlanetIndex,
    string LastElectionId,
    string CurrentElectionId,
    string NextElectionId,
    ulong CurrentElectionEndWarTime,
    int Flags,
    List<TacticalAction> TacticalActions
);
