namespace Helldivers.Models.Domain.War;

/// <summary>
/// Represents a planet and it's current information.
/// </summary>
/// <param name="Index">The numerical ID of this planet.</param>
/// <param name="Name">The human readable name of this planet.</param>
/// <param name="Sector">The name of the sector this planet resides in.</param>
/// <param name="Hash">A hash of this planet.</param>
/// <param name="Position">A set of coordinates where this planet is located.</param>
/// <param name="Waypoints">A list of indices of planets this planet is linked to (we can't link planet objects directly or it would cause circular references).</param>
/// <param name="MaxHealth">The maximum health pool this planet can have.</param>
/// <param name="Health">The current health this planet has, which indicates the control of the faction in <see cref="CurrentOwner" /> (so for humans you want it to go up, otherwise go down).</param>
/// <param name="Disabled">Whether or not this planet is considered active in the current war effort.</param>
/// <param name="InitialOwner">Which faction owned this planet originally.</param>
/// <param name="CurrentOwner">The faction that currently owns this planet.</param>
/// <param name="RegenPerSecond">If left alone, the planet will regenerate X% per second.</param>
/// <param name="Players">The amount of players currently active on this planet.</param>
/// <param name="Statistics">Statistics of this planet (see <see cref="GalacticWar.Statistics" /> for global stats).</param>
public record Planet(
    int Index,
    string Name,
    string Sector,
    long Hash,
    Position Position,
    List<int> Waypoints,
    long MaxHealth,
    long Health,
    bool Disabled,
    string InitialOwner,
    string CurrentOwner,
    double RegenPerSecond,
    long Players,
    Statistics Statistics
);
