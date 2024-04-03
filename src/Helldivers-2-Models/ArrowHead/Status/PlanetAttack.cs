using Helldivers.Models.ArrowHead.Info;

namespace Helldivers.Models.ArrowHead.Status;

/// <summary>
/// Represents an attack on a <see cref="PlanetInfo" />.
/// </summary>
/// <param name="Source">Where the attack originates from.</param>
/// <param name="Target">The planet under attack.</param>
public sealed record PlanetAttack(
    int Source,
    int Target
);
