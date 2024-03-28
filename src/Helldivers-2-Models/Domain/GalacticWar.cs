namespace Helldivers.Models.Domain;

/// <summary>
/// Represents a snapshot of the current ongoing galactic war.
/// </summary>
/// <remarks>
/// This is an abstraction of both warstatus and warinfo to provide
/// an interface that doesn't require understanding the underlying API.
/// </remarks>
public sealed record GalacticWar(
    int WarId,
    DateTime StartedAt,
    DateTime EndsAt,
    DateTime SnapshottedAt,
    double ImpactMultiplier,
    List<Planet> Planets,
    List<Faction> Factions,
    List<Attack> Attacks
);
