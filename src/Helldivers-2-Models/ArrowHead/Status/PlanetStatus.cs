using Helldivers.Models.ArrowHead.Info;

namespace Helldivers.Models.ArrowHead.Status;

/// <summary>
/// Represents the 'current' status of a planet in the galactic war.
/// </summary>
/// <param name="Index">The identifier of the <see cref="PlanetInfo" /> this status refers to.</param>
/// <param name="Owner">The faction currently owning the planet.</param>
/// <param name="Health">The current health / liberation of a planet.</param>
/// <param name="RegenPerSecond">If left alone, how much the health of the planet would regenerate.</param>
/// <param name="Players">The amount of helldivers currently active on this planet.</param>
public sealed record PlanetStatus(
    int Index,
    int Owner,
    long Health,
    double RegenPerSecond,
    ulong Players
);
