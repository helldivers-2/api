namespace Helldivers.Models.Domain.War;

/// <summary>
/// Attacks are launched from one <see cref="Planet" /> onto another.
/// </summary>
/// <param name="From">The <see cref="Planet" /> from which the attack originates.</param>
/// <param name="To">The <see cref="Planet" /> at which the attack is targeted.</param>
public sealed record Attack(
    Planet From,
    Planet To
);
