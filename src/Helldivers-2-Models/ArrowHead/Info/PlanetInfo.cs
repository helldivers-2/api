namespace Helldivers.Models.ArrowHead.Info;

/// <summary>
/// Represents information of a planet from the 'WarInfo' endpoint returned by ArrowHead's API.
/// </summary>
/// <param name="Index">The numerical identifier for this planet, used as reference by other properties throughout the API (like <see cref="Waypoints" />).</param>
/// <param name="SettingsHash">Purpose unknown at this time.</param>
/// <param name="Position">A set of X/Y coordinates specifying the position of this planet on the galaxy map.</param>
/// <param name="Waypoints">A list of links to other planets (supply lines).</param>
/// <param name="Sector">The identifier of the sector this planet is located in.</param>
/// <param name="MaxHealth">The 'health' of this planet, indicates how much liberation it needs to switch sides.</param>
/// <param name="Disabled">Whether this planet is currently considered active in the galactic war.</param>
/// <param name="InitialOwner">The identifier of the faction that initially owned this planet.</param>
public sealed record PlanetInfo(
    int Index,
    long SettingsHash,
    PlanetCoordinates Position,
    List<int> Waypoints,
    int Sector,
    long MaxHealth,
    bool Disabled,
    int InitialOwner
);
